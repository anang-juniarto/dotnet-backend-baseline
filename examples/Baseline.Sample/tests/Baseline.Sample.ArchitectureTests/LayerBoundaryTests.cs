using System.Reflection;
using Baseline.Sample.Application.Features.Items.Commands.CreateItem;
using Baseline.Sample.Application.Features.Items.Queries.GetItem;
using Baseline.Sample.Application.Items;
using Baseline.Sample.Domain.Items;
using Baseline.Sample.Persistence.InMemory.Items;
using MediatR;
using Xunit;

namespace Baseline.Sample.ArchitectureTests;

/// <summary>Enforces compiled assembly dependencies and core type contracts without an architecture testing library.</summary>
public sealed class LayerBoundaryTests
{
    /// <summary>Domain references only runtime assemblies, never application, transport or provider SDKs.</summary>
    [Fact]
    public void DomainDependsOnlyOnRuntime()
    {
        var references = typeof(Item).Assembly.GetReferencedAssemblies();
        Assert.NotEmpty(references);
        Assert.All(references, reference => Assert.True(IsRuntime(reference.Name!), reference.FullName));
    }

    /// <summary>Application may depend on domain and its approved orchestration libraries but not outer layers.</summary>
    [Fact]
    public void ApplicationDependenciesPointInward()
    {
        var references = typeof(IItemStore).Assembly.GetReferencedAssemblies();
        Assert.Contains(references, reference => reference.Name == "Baseline.Sample.Domain");
        Assert.All(references, reference => Assert.True(
            IsRuntime(reference.Name!) || reference.Name == "Baseline.Sample.Domain" ||
            reference.Name!.StartsWith("MediatR", StringComparison.Ordinal) ||
            reference.Name.StartsWith("FluentValidation", StringComparison.Ordinal) ||
            reference.Name.StartsWith("Microsoft.Extensions.", StringComparison.Ordinal), reference.FullName));
    }

    /// <summary>The in-memory implementation depends on core contracts and never on presentation or relational SDKs.</summary>
    [Fact]
    public void InMemoryAdapterDependsOnlyOnCoreAndRuntime()
    {
        var references = typeof(InMemoryItemStore).Assembly.GetReferencedAssemblies();
        Assert.Contains(references, reference => reference.Name == "Baseline.Sample.Application");
        Assert.All(references, reference => Assert.True(
            IsRuntime(reference.Name!) || reference.Name is "Baseline.Sample.Domain" or "Baseline.Sample.Application" ||
            reference.Name!.StartsWith("Microsoft.Extensions.", StringComparison.Ordinal), reference.FullName));
        Assert.True(typeof(IItemStore).IsAssignableFrom(typeof(InMemoryItemStore)));
    }

    /// <summary>Application requests and handlers retain their strongly typed mediator boundary.</summary>
    [Fact]
    public void FeatureHandlersImplementTypedMediatorContracts()
    {
        Assert.True(typeof(IRequest<ItemDto>).IsAssignableFrom(typeof(CreateItemCommand)));
        Assert.True(typeof(IRequest<ItemDto>).IsAssignableFrom(typeof(GetItemQuery)));
        Assert.True(typeof(IRequestHandler<CreateItemCommand, ItemDto>).IsAssignableFrom(typeof(CreateItemCommandHandler)));
        Assert.True(typeof(IRequestHandler<GetItemQuery, ItemDto?>).IsAssignableFrom(typeof(GetItemQueryHandler)));
        foreach (var handler in new[] { typeof(CreateItemCommandHandler), typeof(GetItemQueryHandler) })
        {
            var constructor = Assert.Single(handler.GetConstructors());
            Assert.Equal(typeof(IItemStore), Assert.Single(constructor.GetParameters()).ParameterType);
        }
    }

    /// <summary>Core public contracts cannot expose provider SDK or transport types through signatures.</summary>
    [Fact]
    public void CorePublicSignaturesDoNotLeakOuterLayerTypes()
    {
        foreach (var assembly in new[] { typeof(Item).Assembly, typeof(IItemStore).Assembly })
            foreach (var type in assembly.GetExportedTypes())
            {
                var signatureTypes = type.GetInterfaces()
                    .Concat(type.GetConstructors().SelectMany(constructor => constructor.GetParameters().Select(parameter => parameter.ParameterType)))
                    .Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(property => property.PropertyType))
                    .Concat(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(field => field.FieldType))
                    .Concat(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly)
                        .SelectMany(method => method.GetParameters().Select(parameter => parameter.ParameterType).Append(method.ReturnType)));
                foreach (var signature in signatureTypes.SelectMany(ExpandType))
                {
                    var name = signature.Assembly.GetName().Name!;
                    Assert.False(name.StartsWith("Baseline.Sample.Persistence.", StringComparison.Ordinal) ||
                        name.StartsWith("Baseline.Sample.WebApi", StringComparison.Ordinal) ||
                        name.StartsWith("Microsoft.AspNetCore.", StringComparison.Ordinal) ||
                        name.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) ||
                        name.StartsWith("MongoDB", StringComparison.Ordinal) ||
                        name.StartsWith("StackExchange.Redis", StringComparison.Ordinal) ||
                        name.StartsWith("Elastic.", StringComparison.Ordinal), $"{type.FullName} exposes {signature.FullName}");
                }
            }
    }

    /// <summary>The default API references only its in-memory profile, framework and approved core orchestration libraries.</summary>
    [Fact]
    public void DefaultWebApiDoesNotReferenceOptionalModulesOrVendorAssemblies()
    {
        var references = typeof(Program).Assembly.GetReferencedAssemblies();
        Assert.Contains(references, reference => reference.Name == "Baseline.Sample.Application");
        Assert.Contains(references, reference => reference.Name == "Baseline.Sample.Persistence.InMemory");
        Assert.All(references, reference => Assert.True(
            IsRuntime(reference.Name!) ||
            reference.Name is "Baseline.Sample.Application" or "Baseline.Sample.Domain" or "Baseline.Sample.Persistence.InMemory" or "Microsoft.AspNetCore" ||
            reference.Name!.StartsWith("Microsoft.AspNetCore.", StringComparison.Ordinal) ||
            reference.Name.StartsWith("Microsoft.Extensions.", StringComparison.Ordinal) ||
            reference.Name.StartsWith("MediatR", StringComparison.Ordinal) ||
            reference.Name.StartsWith("FluentValidation", StringComparison.Ordinal), reference.FullName));
    }

    /// <summary>Domain, application, persistence and host remain separate compiled assemblies.</summary>
    [Fact]
    public void LayersHaveDistinctAssemblies()
    {
        var assemblies = new[] { typeof(Item).Assembly, typeof(IItemStore).Assembly, typeof(InMemoryItemStore).Assembly, typeof(Program).Assembly };
        Assert.Equal(4, assemblies.Distinct().Count());
        Assert.Equal("Baseline.Sample.WebApi", typeof(Program).Assembly.GetName().Name);
    }

    /// <summary>Recognizes framework runtime dependencies rather than treating every System-prefixed package as a provider.</summary>
    private static bool IsRuntime(string name) =>
        name == "netstandard" || name == "System" || name.StartsWith("System.", StringComparison.Ordinal);

    /// <summary>Traverses generic arguments and element types so wrapped SDK types cannot bypass the boundary check.</summary>
    private static IEnumerable<Type> ExpandType(Type type)
    {
        yield return type;
        if (type.HasElementType)
            foreach (var element in ExpandType(type.GetElementType()!))
                yield return element;
        foreach (var argument in type.GetGenericArguments())
            foreach (var nested in ExpandType(argument))
                yield return nested;
    }
}
