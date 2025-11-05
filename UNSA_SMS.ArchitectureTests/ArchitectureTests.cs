namespace UNSA_SMS.ArchitectureTests;

using NetArchTest.Rules;
using System.Security.Principal;
using Xunit;

public class ArchitectureTests
{
    private static System.Reflection.Assembly GetAssembly(System.Type type) => type.Assembly;

    [Theory]
    [InlineData(typeof(Identity.Core.Class1), typeof(Identity.Application.Class1))]
    [InlineData(typeof(Faculty.Core.Class1), typeof(Faculty.Application.Class1))]
    [InlineData(typeof(Analytics.Core.Class1), typeof(Analytics.Application.Class1))]
    [InlineData(typeof(Notifications.Core.Class1), typeof(Notifications.Application.Class1))]
    [InlineData(typeof(Support.Core.Class1), typeof(Support.Application.Class1))]
    [InlineData(typeof(University.Core.Class1), typeof(University.Application.Class1))]
    public void Core_ShouldNotDependOn_Application(System.Type coreType, System.Type appType)
    {
        var coreAssembly = GetAssembly(coreType);
        var appAssemblyName = GetAssembly(appType).GetName().Name;

        var result = Types.InAssembly(coreAssembly)
            .Should()
            .NotHaveDependencyOn(appAssemblyName)
            .GetResult();

        Assert.True(result.IsSuccessful, $"{coreAssembly.GetName().Name} should not depend on {appAssemblyName}");
    }

    [Theory]
    [InlineData(typeof(Identity.Application.Class1), typeof(Identity.Infrastructure.Class1))]
    [InlineData(typeof(Faculty.Application.Class1), typeof(Faculty.Infrastructure.Class1))]
    [InlineData(typeof(Analytics.Application.Class1), typeof(Analytics.Infrastructure.Class1))]
    [InlineData(typeof(Notifications.Application.Class1), typeof(Notifications.Infrastructure.Class1))]
    [InlineData(typeof(Support.Application.Class1), typeof(Support.Infrastructure.Class1))]
    [InlineData(typeof(University.Application.Class1), typeof(University.Infrastructure.Class1))]
    public void Application_ShouldNotDependOn_Infrastructure(System.Type appType, System.Type infraType)
    {
        var appAssembly = GetAssembly(appType);
        var infraAssemblyName = GetAssembly(infraType).GetName().Name;

        var result = Types.InAssembly(appAssembly)
            .Should()
            .NotHaveDependencyOn(infraAssemblyName)
            .GetResult();

        Assert.True(result.IsSuccessful, $"{appAssembly.GetName().Name} should not depend on {infraAssemblyName}");
    }

    [Theory]
    [InlineData(typeof(Analytics.Application.Class1), "Analytics.Application.DTOs")]
    [InlineData(typeof(Faculty.Application.Class1), "Faculty.Application.DTOs")]
    [InlineData(typeof(Identity.Application.Class1), "Identity.Application.DTOs")]
    [InlineData(typeof(Notifications.Application.Class1), "Notifications.Application.DTOs")]
    [InlineData(typeof(Support.Application.Class1), "Support.Application.DTOs")]
    [InlineData(typeof(University.Application.Class1), "University.Application.DTOs")]
    public void DtoClasses_ShouldHaveNameEndingWith_DTO(System.Type appType, string dtoNamespace)
    {
        var appAssembly = GetAssembly(appType);

        var result = Types.InAssembly(appAssembly)
            .That()
            .ResideInNamespace(dtoNamespace)
            .Should()
            .HaveNameEndingWith("DTO")
            .GetResult();

        Assert.True(result.IsSuccessful, $"All DTO classes in {dtoNamespace} should end with 'DTO'");
    }

    [Theory]
    [InlineData(typeof(Analytics.Application.Class1), "Analytics.Application.Services")]
    [InlineData(typeof(Faculty.Application.Class1), "Faculty.Application.Services")]
    [InlineData(typeof(Identity.Application.Class1), "Identity.Application.Services")]
    [InlineData(typeof(Notifications.Application.Class1), "Notifications.Application.Services")]
    [InlineData(typeof(Support.Application.Class1), "Support.Application.Services")]
    [InlineData(typeof(University.Application.Class1), "University.Application.Services")]
    public void Services_ShouldHaveNameEndingWith_Service(System.Type appType, string serviceNamespace)
    {
        var appAssembly = GetAssembly(appType);

        var result = Types.InAssembly(appAssembly)
            .That()
            .ResideInNamespace(serviceNamespace)
            .Should()
            .HaveNameEndingWith("Service")
            .GetResult();

        Assert.True(result.IsSuccessful, $"All services in {serviceNamespace} should end with 'Service'");
    }

    [Theory]
    [InlineData(typeof(Analytics.Core.Class1), "Analytics.Core.Interfaces")]
    [InlineData(typeof(Faculty.Core.Class1), "Faculty.Core.Interfaces")]
    [InlineData(typeof(Identity.Core.Class1), "Identity.Core.Interfaces")]
    [InlineData(typeof(Notifications.Core.Class1), "Notifications.Core.Interfaces")]
    [InlineData(typeof(Support.Core.Class1), "Support.Core.Interfaces")]
    [InlineData(typeof(University.Core.Class1), "University.Core.Interfaces")]
    public void Interfaces_ShouldHaveNameStartingWith_I(System.Type appType, string interfaceNamespace)
    {
        var assembly = GetAssembly(appType);

        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace(interfaceNamespace)
            .And()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        Assert.True(result.IsSuccessful, $"All interfaces in {interfaceNamespace} should start with 'I'");
    }

    [Theory]
    [InlineData(typeof(Analytics.Application.Class1), "Analytics.Application.Services")]
    [InlineData(typeof(Faculty.Application.Class1), "Faculty.Application.Services")]
    [InlineData(typeof(Identity.Application.Class1), "Identity.Application.Services")]
    [InlineData(typeof(Notifications.Application.Class1), "Notifications.Application.Services")]
    [InlineData(typeof(Support.Application.Class1), "Support.Application.Services")]
    [InlineData(typeof(University.Application.Class1), "University.Application.Services")]
    public void Services_ShouldBeClasses(System.Type appType, string serviceNamespace)
    {
        var assembly = GetAssembly(appType);

        var result = Types.InAssembly(assembly)
            .That()
            .ResideInNamespace(serviceNamespace)
            .Should()
            .BeClasses()
            .GetResult();

        Assert.True(result.IsSuccessful, $"All types in {serviceNamespace} should be classes");
    }
}
