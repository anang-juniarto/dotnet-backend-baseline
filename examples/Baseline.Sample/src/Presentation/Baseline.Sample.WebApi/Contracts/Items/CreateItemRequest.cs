using System.ComponentModel.DataAnnotations;

namespace Baseline.Sample.WebApi.Contracts.Items;

/// <summary>Accepts a display name; ownership is resolved from the authenticated principal.</summary>
public sealed class CreateItemRequest
{
    /// <summary>Display name evaluated by Application and Domain after trimming; the 1 to 100 character length invariant is enforced after whitespace normalization.</summary>
    [Required]
    public string? Name { get; init; }
}
