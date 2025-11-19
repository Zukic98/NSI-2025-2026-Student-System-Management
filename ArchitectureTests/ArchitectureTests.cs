using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Architecture.Tests
{
    public class ArchitectureTests
    {
        private const string Analytics = "Analytics";
        private const string Common = "Common";
        private const string Faculty = "Faculty";
        private const string Identity = "Identity";
        private const string Notifications = "Notifications";
        private const string Support = "Support";
        private const string University = "University";

        private static readonly Assembly AnalyticsApplication = typeof(Analytics.Application.Class1).Assembly;
        private static readonly Assembly AnalyticsCore = typeof(Analytics.Core.Class1).Assembly;
        private static readonly Assembly AnalyticsInfrastructure = typeof(Analytics.Infrastructure.Class1).Assembly;
        private static readonly Assembly AnalyticsApi = typeof(Analytics.API.Controllers.AnalyticsController).Assembly;

        private static readonly Assembly CommonApplication = typeof(Common.Application.Class1).Assembly;
        private static readonly Assembly CommonCore = typeof(Common.Core.Class1).Assembly;
        private static readonly Assembly CommonInfrastructure = typeof(Common.Infrastructure.Class1).Assembly;
        private static readonly Assembly CommonApi = typeof(Common.API.Class1).Assembly;

        private static readonly Assembly FacultyApplication = typeof(Faculty.Application.Class1).Assembly;
        private static readonly Assembly FacultyCore = typeof(Faculty.Core.Class1).Assembly;
        private static readonly Assembly FacultyInfrastructure = typeof(Faculty.Infrastructure.Class1).Assembly;
        private static readonly Assembly FacultyApi = typeof(Faculty.API.Controllers.FacultyController).Assembly;

        private static readonly Assembly IdentityApplication = typeof(Identity.Application.Class1).Assembly;
        private static readonly Assembly IdentityCore = typeof(Identity.Core.Class1).Assembly;
        private static readonly Assembly IdentityInfrastructure = typeof(Identity.Infrastructure.Class1).Assembly;
        private static readonly Assembly IdentityApi = typeof(Identity.API.Controllers.IdentityController).Assembly;

        private static readonly Assembly NotificationsApplication = typeof(Notifications.Application.Class1).Assembly;
        private static readonly Assembly NotificationsCore = typeof(Notifications.Core.Class1).Assembly;
        private static readonly Assembly NotificationsInfrastructure = typeof(Notifications.Infrastructure.Class1).Assembly;
        private static readonly Assembly NotificationsApi = typeof(Notifications.API.Controllers.NotificationsController).Assembly;

        private static readonly Assembly SupportApplication = typeof(Support.Application.Class1).Assembly;
        private static readonly Assembly SupportCore = typeof(Support.Core.Class1).Assembly;
        private static readonly Assembly SupportInfrastructure = typeof(Support.Infrastructure.Class1).Assembly;
        private static readonly Assembly SupportApi = typeof(Support.API.Controllers.SupportController).Assembly;

        private static readonly Assembly UniversityApplication = typeof(University.Application.Class1).Assembly;
        private static readonly Assembly UniversityCore = typeof(University.Core.Class1).Assembly;
        private static readonly Assembly UniversityInfrastructure = typeof(University.Infrastructure.Class1).Assembly;
        private static readonly Assembly UniversityApi = typeof(University.API.Controllers.UniversityController).Assembly;

        // ---------- Helpers: imena assembly-ja (string array) ----------
        private static string[] AllApplicationAssemblyNames() => new[]
        {
            AnalyticsApplication.GetName().Name,
            CommonApplication.GetName().Name,
            FacultyApplication.GetName().Name,
            IdentityApplication.GetName().Name,
            NotificationsApplication.GetName().Name,
            SupportApplication.GetName().Name,
            UniversityApplication.GetName().Name
        };

        private static string[] AllInfrastructureAssemblyNames() => new[]
        {
            AnalyticsInfrastructure.GetName().Name,
            CommonInfrastructure.GetName().Name,
            FacultyInfrastructure.GetName().Name,
            IdentityInfrastructure.GetName().Name,
            NotificationsInfrastructure.GetName().Name,
            SupportInfrastructure.GetName().Name,
            UniversityInfrastructure.GetName().Name
        };

        private static string[] AllApiAssemblyNames() => new[]
        {
            AnalyticsApi.GetName().Name,
            CommonApi.GetName().Name,
            FacultyApi.GetName().Name,
            IdentityApi.GetName().Name,
            NotificationsApi.GetName().Name,
            SupportApi.GetName().Name,
            UniversityApi.GetName().Name
        };

        // ---------- TESTS ----------
        [Fact]
        public void Core_Should_Not_Depend_On_Application()
        {
            var coreAssemblies = new[]
            {
                AnalyticsCore, CommonCore, FacultyCore, IdentityCore, NotificationsCore, SupportCore, UniversityCore
            };

            var result = Types.InAssemblies(coreAssemblies)
                .Should()
                .NotHaveDependencyOnAll(AllApplicationAssemblyNames())
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Core_Should_Not_Depend_On_Infrastructure()
        {
            var coreAssemblies = new[]
            {
                AnalyticsCore, CommonCore, FacultyCore, IdentityCore, NotificationsCore, SupportCore, UniversityCore
            };

            var result = Types.InAssemblies(coreAssemblies)
                .Should()
                .NotHaveDependencyOnAll(AllInfrastructureAssemblyNames())
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_Depend_On_Infrastructure()
        {
            var appAssemblies = new[]
            {
                AnalyticsApplication, CommonApplication, FacultyApplication, IdentityApplication, NotificationsApplication, SupportApplication, UniversityApplication
            };

            var result = Types.InAssemblies(appAssemblies)
                .Should()
                .NotHaveDependencyOnAll(AllInfrastructureAssemblyNames())
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Core_Should_Not_Depend_On_Api()
        {
            var coreAssemblies = new[]
            {
                AnalyticsCore, CommonCore, FacultyCore, IdentityCore, NotificationsCore, SupportCore, UniversityCore
            };

            var result = Types.InAssemblies(coreAssemblies)
                .Should()
                .NotHaveDependencyOnAll(AllApiAssemblyNames())
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_Depend_On_Api()
        {
            var appAssemblies = new[]
            {
                AnalyticsApplication, CommonApplication, FacultyApplication, IdentityApplication, NotificationsApplication, SupportApplication, UniversityApplication
            };

            var result = Types.InAssemblies(appAssemblies)
                .Should()
                .NotHaveDependencyOnAll(AllApiAssemblyNames())
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_Depend_On_Api()
        {
            var infraAssemblies = new[]
            {
                AnalyticsInfrastructure, CommonInfrastructure, FacultyInfrastructure, IdentityInfrastructure, NotificationsInfrastructure, SupportInfrastructure, UniversityInfrastructure
            };

            var result = Types.InAssemblies(infraAssemblies)
                .Should()
                .NotHaveDependencyOnAll(AllApiAssemblyNames())
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Controllers_Should_Inherit_ControllerBase_And_EndWith_Controller()
        {
            var apiAssemblies = new[]
            {
                AnalyticsApi, CommonApi, FacultyApi, IdentityApi, NotificationsApi, SupportApi, UniversityApi
            };

            var result = Types.InAssemblies(apiAssemblies)
                .That()
                .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
                .Should()
                .BeClasses()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();

            var nameResult = Types.InAssemblies(apiAssemblies)
                .That()
                .Inherit(typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
                .Should()
                .HaveNameEndingWith("Controller")
                .GetResult();

            nameResult.IsSuccessful.Should().BeTrue();
        }
    }
}
