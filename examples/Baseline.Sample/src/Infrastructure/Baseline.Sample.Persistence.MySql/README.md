# MySQL persistence: deferred

The canonical blueprint selects **Pomelo.EntityFrameworkCore.MySql**. The NuGet version index inspected on 2026-10-10 ends at **9.0.0**, with no EF Core 10 release. This directory deliberately has no executable project, registration method, or invented migrations.

Oracle **MySql.EntityFrameworkCore 10.0.9** does publish a `net10.0` dependency group requiring EF Core/Relational **10.0.9** and MySql.Data **26.7.0**. It is a distinct provider with a different license (`GPL-2.0-only WITH Universal-FOSS-exception-1.0`), not a drop-in approved Pomelo selection. Adopting it requires an explicit provider/license decision and separate implementation and verification; this document does not claim all MySQL providers lack EF10 support.

Re-evaluate Pomelo release compatibility before enabling this module. No database connections, migrations, or MySQL runtime checks were performed.

Evidence:
- https://api.nuget.org/v3-flatcontainer/pomelo.entityframeworkcore.mysql/index.json
- https://api.nuget.org/v3-flatcontainer/mysql.entityframeworkcore/10.0.9/mysql.entityframeworkcore.nuspec
