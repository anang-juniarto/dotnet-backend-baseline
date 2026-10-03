[CmdletBinding()]
param (
    [Parameter(Mandatory = $true)]
    [string]$TargetRoot
)

$commonScript = Join-Path $PSScriptRoot "adoption-common.ps1"
if (Test-Path -LiteralPath $commonScript) {
    . $commonScript
}

if (-not (Test-Path -LiteralPath $TargetRoot)) {
    Write-Error "Target root does not exist: $TargetRoot"
    exit 1
}

$canonicalTarget = (Resolve-Path $TargetRoot).Path
$cacheDir = Join-Path $canonicalTarget ".kilo\cache\graphify"

if (-not (Test-Path -LiteralPath $cacheDir)) {
    New-Item -ItemType Directory -Path $cacheDir -Force | Out-Null
}

$summaryPath = Join-Path $cacheDir "summary.json"

# 1. Discover Solution / Project Manifests & Compute Fingerprint
$manifests = Get-ChildItem -LiteralPath $canonicalTarget -Recurse -File -Include "*.sln", "*.slnx", "*.csproj" -ErrorAction SilentlyContinue | Where-Object {
    $_.FullName -notmatch "(\.kilo|node_modules|bin|obj)"
}

$nowUtc = [System.DateTime]::UtcNow.ToString("o")

# Fingerprint based on manifest count and write times
$manifestFingerprint = ""
if ($manifests -and $manifests.Count -gt 0) {
    $fpParts = $manifests | Sort-Object FullName | ForEach-Object { "$($_.Name):$($_.LastWriteTimeUtc.Ticks)" }
    $fpBytes = [System.Text.Encoding]::UTF8.GetBytes(($fpParts -join ";"))
    $hasher = [System.Security.Cryptography.SHA256]::Create()
    $manifestFingerprint = [System.BitConverter]::ToString($hasher.ComputeHash($fpBytes)).Replace('-', '').Substring(0, 16)
}

if (-not $manifests -or $manifests.Count -eq 0) {
    $summary = [ordered]@{
        Status              = "pending_source"
        Message             = "No solution or project manifests found. Graphify indexing deferred until source exists."
        GeneratedUtc        = $nowUtc
        TargetRoot          = $canonicalTarget
        ManifestCount       = 0
        ManifestFingerprint = $null
        NodeCount           = 0
        EdgeCount           = 0
        Cycles              = "not_computed"
    }
    $summaryJson = $summary | ConvertTo-Json -Depth 5
    [System.IO.File]::WriteAllText($summaryPath, $summaryJson, [System.Text.Encoding]::UTF8)
    Write-Output "GRAPHIFY_STATUS: pending_source"
    return
}

# 2. Verify Graphify Tool Installation
$graphifyCmd = Get-Command "graphify" -ErrorAction SilentlyContinue

if (-not $graphifyCmd) {
    $summary = [ordered]@{
        Status              = "unavailable"
        Message             = "Graphify CLI is not installed in the environment. Using manifest and project-reference fallback."
        GeneratedUtc        = $nowUtc
        TargetRoot          = $canonicalTarget
        ManifestCount       = $manifests.Count
        ManifestFingerprint = $manifestFingerprint
        NodeCount           = 0
        EdgeCount           = 0
        Cycles              = "not_computed"
    }
    $summaryJson = $summary | ConvertTo-Json -Depth 5
    [System.IO.File]::WriteAllText($summaryPath, $summaryJson, [System.Text.Encoding]::UTF8)
    Write-Output "GRAPHIFY_STATUS: unavailable (manifest fallback active)"
    return
}

# 3. Execute Graphify Code-Only Extraction
try {
    Write-Output "Executing Graphify extraction on: $canonicalTarget..."
    $toolVersion = (graphify --version 2>$null | Select-Object -First 1)

    # Run native extraction
    & graphify extract $canonicalTarget --code-only --output $cacheDir --no-cluster
    $exitCode = $LASTEXITCODE

    if ($exitCode -ne 0) {
        $summary = [ordered]@{
            Status              = "failed"
            Message             = "Graphify process exited with non-zero exit code $exitCode."
            ExitCode            = $exitCode
            GeneratedUtc        = $nowUtc
            TargetRoot          = $canonicalTarget
            ManifestFingerprint = $manifestFingerprint
            NodeCount           = 0
            EdgeCount           = 0
        }
        $summaryJson = $summary | ConvertTo-Json -Depth 5
        [System.IO.File]::WriteAllText($summaryPath, $summaryJson, [System.Text.Encoding]::UTF8)
        Write-Warning "GRAPHIFY_STATUS: failed (exit code $exitCode)"
        return
    }

    # Verify Output File & Validate JSON Integrity
    $graphJsonPath = Join-Path $cacheDir "graphify-out\graph.json"
    if (-not (Test-Path -LiteralPath $graphJsonPath)) {
        $summary = [ordered]@{
            Status              = "failed"
            Message             = "Graphify completed with exit code 0 but expected output graph.json was not found at: $graphJsonPath"
            GeneratedUtc        = $nowUtc
            TargetRoot          = $canonicalTarget
            ManifestFingerprint = $manifestFingerprint
            NodeCount           = 0
            EdgeCount           = 0
        }
        $summaryJson = $summary | ConvertTo-Json -Depth 5
        [System.IO.File]::WriteAllText($summaryPath, $summaryJson, [System.Text.Encoding]::UTF8)
        Write-Warning "GRAPHIFY_STATUS: failed (output file missing)"
        return
    }

    $rawGraph = Get-Content -LiteralPath $graphJsonPath -Raw -Encoding utf8
    $parsedGraph = $rawGraph | ConvertFrom-Json

    $nodeCount = if ($parsedGraph.nodes) { $parsedGraph.nodes.Count } else { 0 }
    $edgeCount = if ($parsedGraph.edges) { $parsedGraph.edges.Count } else { 0 }

    $summary = [ordered]@{
        Status              = "indexed"
        ToolVersion         = $toolVersion
        GeneratedUtc        = $nowUtc
        TargetRoot          = $canonicalTarget
        OutputPath          = $graphJsonPath
        ManifestCount       = $manifests.Count
        ManifestFingerprint = $manifestFingerprint
        NodeCount           = $nodeCount
        EdgeCount           = $edgeCount
        Cycles              = "not_computed"
    }

    $summaryJson = $summary | ConvertTo-Json -Depth 5
    [System.IO.File]::WriteAllText($summaryPath, $summaryJson, [System.Text.Encoding]::UTF8)
    Write-Output "GRAPHIFY_STATUS: indexed ($nodeCount nodes, $edgeCount edges)"
} catch {
    $summary = [ordered]@{
        Status              = "failed"
        Message             = "Exception during Graphify execution: $($_.Exception.Message)"
        GeneratedUtc        = $nowUtc
        TargetRoot          = $canonicalTarget
        ManifestFingerprint = $manifestFingerprint
        NodeCount           = 0
        EdgeCount           = 0
    }
    $summaryJson = $summary | ConvertTo-Json -Depth 5
    [System.IO.File]::WriteAllText($summaryPath, $summaryJson, [System.Text.Encoding]::UTF8)
    Write-Warning "GRAPHIFY_STATUS: failed with exception: $($_.Exception.Message)"
}
