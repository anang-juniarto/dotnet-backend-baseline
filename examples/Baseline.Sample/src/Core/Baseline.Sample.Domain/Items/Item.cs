namespace Baseline.Sample.Domain.Items;

/// <summary>An immutable item owned by one local demo identity.</summary>
public sealed class Item
{
    /// <summary>The stable identifier allocated when the item is created.</summary>
    public Guid Id { get; }

    /// <summary>The case-sensitive identity permitted to retrieve this item.</summary>
    public string OwnerId { get; }

    /// <summary>The trimmed display name, containing between 1 and 100 characters.</summary>
    public string Name { get; }

    /// <summary>Creates an item after enforcing identity and name invariants.</summary>
    public Item(Guid id, string ownerId, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("An item identifier is required.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var normalizedName = name.Trim();
        if (normalizedName.Length > 100)
        {
            throw new ArgumentException("The item name must contain at most 100 characters.", nameof(name));
        }

        Id = id;
        OwnerId = ownerId;
        Name = normalizedName;
    }
}
