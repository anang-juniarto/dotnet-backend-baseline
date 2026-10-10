using Baseline.Sample.Domain.Items;
using Xunit;

namespace Baseline.Sample.UnitTests;

/// <summary>Exercises domain invariants independently of storage and transport.</summary>
public sealed class ItemTests
{
    /// <summary>Valid names are normalized without changing owner identity.</summary>
    [Theory]
    [InlineData(" x ", "x")]
    [InlineData("sample", "sample")]
    public void NamesAreTrimmed(string input, string expected)
    {
        var item = new Item(Guid.NewGuid(), "Alice", input);
        Assert.Equal(expected, item.Name);
        Assert.Equal("Alice", item.OwnerId);
    }

    /// <summary>Names outside the bounded nonempty contract are rejected.</summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void BlankNamesAreRejected(string name) =>
        Assert.ThrowsAny<ArgumentException>(() => new Item(Guid.NewGuid(), "alice", name));

    /// <summary>Both boundaries are accepted and overlength input is rejected.</summary>
    [Fact]
    public void NameLengthIsBounded()
    {
        Assert.Equal(1, new Item(Guid.NewGuid(), "alice", "x").Name.Length);
        Assert.Equal(100, new Item(Guid.NewGuid(), "alice", new string('x', 100)).Name.Length);
        Assert.Throws<ArgumentException>(() => new Item(Guid.NewGuid(), "alice", new string('x', 101)));
    }

    /// <summary>Empty resource identifiers and missing owners cannot enter storage.</summary>
    [Fact]
    public void IdentityIsRequired()
    {
        Assert.Throws<ArgumentException>(() => new Item(Guid.Empty, "alice", "x"));
        Assert.ThrowsAny<ArgumentException>(() => new Item(Guid.NewGuid(), " ", "x"));
    }
}
