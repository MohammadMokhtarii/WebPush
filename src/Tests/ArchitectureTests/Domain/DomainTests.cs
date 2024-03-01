using Core.Domain;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureTests.Domain;

public class DomainTests
{
    private static readonly Assembly DomainAssembly = typeof(Entity).Assembly;

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        var result = Types.InAssembly(DomainAssembly)
                          .That()
                          .ImplementInterface(typeof(IDomainEvent))
                          .Should()
                          .BeSealed()
                          .GetResult();

        result.IsSuccessful.Should()
                           .BeTrue();
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        var result = Types.InAssembly(DomainAssembly)
                          .That()
                          .ImplementInterface(typeof(IDomainEvent))
                          .Should()
                          .HaveNameEndingWith("DomainEvent")
                          .GetResult();

        result.IsSuccessful.Should()
                           .BeTrue();
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstrucor()
    {
        var entityTypes = Types.InAssembly(DomainAssembly)
                               .That()
                               .Inherit(typeof(Entity))
                               .GetTypes();

        List<Type> failingTypes = [];
        foreach (var entityType in entityTypes)
        {
            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            if (!constructors.Any(x => x.IsPrivate && x.GetParameters().Length == 0))
                failingTypes.Add(entityType);
        }
        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_Should_BeSealed()
    {
        var result = Types.InAssembly(DomainAssembly)
                          .That()
                          .Inherit(typeof(Entity))
                          .Should()
                          .BeSealed()
                          .GetResult();

        result.IsSuccessful.Should()
                           .BeTrue();
    }

    //[Fact]
    //public void Entities_Should_HaveStaticFactoryMethod_When_HaveNonParameterlessConstrucor()
    //{
    //    var entityTypes = Types.InAssembly(DomainAssembly)
    //                           .That()
    //                           .Inherit(typeof(Entity))
    //                           .GetTypes();

    //    List<Type> failingTypes = [];
    //    foreach (var entityType in entityTypes)
    //    {
    //        var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
    //        if (constructors.Any(x => x.IsPrivate && x.GetParameters().Length != 0))
    //        {
    //            var methods = entityType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
    //            if (!methods.Any(x => x.ReturnType == entityType && x.Name == "Create"))
    //                failingTypes.Add(entityType);

    //        }
    //    }
    //    failingTypes.Should().BeEmpty();
    //}

}
