using Shouldly;
using NetArchTest.Rules;
using System.Reflection;

namespace Tests
{
    public class ArchitectureTests
    {
        private static readonly string[] Modules =
        [
            "Analytics",
            "Identity",
            "Faculty",
            "University",
            "Notifications",
            "Support"
        ];

        public static IEnumerable<object[]> ModuleNames()
        {
            foreach (var module in Modules)
                yield return new object[] { module };
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Api_ShouldNotDependOn_Infrastructure(string module)
        {
            var api = Assembly.Load($"{module}.API");
            var infra = Assembly.Load($"{module}.Infrastructure");

            var result = Types.InAssembly(api)
                .Should()
                .NotHaveDependencyOn(infra.GetName().Name)
                .GetResult();

            result.IsSuccessful.ShouldBeTrue($"{module}: API should not depend on Infrastructure");
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Core_ShouldNotDependOn_ApplicationInfrastructureOrAPI(string module)
        {
            var core = Assembly.Load($"{module}.Core");
            var app = Assembly.Load($"{module}.Application");
            var infra = Assembly.Load($"{module}.Infrastructure");
            var api = Assembly.Load($"{module}.API");

            Types.InAssembly(core)
                .Should()
                .NotHaveDependencyOn(app.GetName().Name)
                .GetResult()
                .IsSuccessful.ShouldBeTrue($"{module}: Core should not depend on Application");

            Types.InAssembly(core)
                .Should()
                .NotHaveDependencyOn(infra.GetName().Name)
                .GetResult()
                .IsSuccessful.ShouldBeTrue($"{module}: Core should not depend on Infrastructure");

            Types.InAssembly(core)
                .Should()
                .NotHaveDependencyOn(api.GetName().Name)
                .GetResult()
                .IsSuccessful.ShouldBeTrue($"{module}: Core should not depend on API");
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Application_ShouldNotDependOnAPIOrInfrastructure(string module)
        {
            var app = Assembly.Load($"{module}.Application");
            var infra = Assembly.Load($"{module}.Infrastructure");
            var api = Assembly.Load($"{module}.API");

            Types.InAssembly(app)
                .Should()
                .NotHaveDependencyOn(api.GetName().Name)
                .GetResult()
                .IsSuccessful.ShouldBeTrue($"{module}: Application should not depend on API");

            Types.InAssembly(app)
                .Should()
                .NotHaveDependencyOn(infra.GetName().Name)
                .GetResult()
                .IsSuccessful.ShouldBeTrue($"{module}: Application should not depend on Infrastructure");
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Infrastructure_ShouldNotDependOn_API(string module)
        {
            var infra = Assembly.Load($"{module}.Infrastructure");
            var api = Assembly.Load($"{module}.API");

            Types.InAssembly(infra)
                .Should()
                .NotHaveDependencyOn(api.GetName().Name)
                .GetResult()
                .IsSuccessful.ShouldBeTrue($"{module}: Infrastructure should not depend on API");
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void CoreInterfaces_ShouldHaveImplementations(string module)
        {
            var coreAssembly = Assembly.Load($"{module}.Core");
            var appAssembly = Assembly.Load($"{module}.Application");

            var coreInterfaces = coreAssembly
                .GetTypes()
                .Where(t => t.IsInterface && !t.IsGenericType)
                .ToList();

            var implementations = appAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var iface in coreInterfaces)
            {
                bool implemented = implementations.Any(c => iface.IsAssignableFrom(c));
                implemented.ShouldBeTrue(
                    $"{module}: Interface {iface.FullName} has no implementation in Application."
                );
            }
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void CoreInterfaces_ShouldResideInInterfacesNamespace(string module)
        {
            var coreAssembly = Assembly.Load($"{module}.Core");

            var interfaces = coreAssembly.GetTypes().Where(t => t.IsInterface).ToList();

            foreach (var iface in interfaces)
            {
                iface.Namespace.ShouldNotBeNull($"{module}: Interface {iface.Name} has no namespace.");

                iface.Namespace.ShouldContain(".Interfaces", Case.Sensitive, $"{module}: Interface {iface.FullName} is not inside the '.Interfaces' namespace/folder.");
            }
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Controllers_ShouldBeSuffixed_WithController(string module)
        {
            var api = Assembly.Load($"{module}.API");

            var controllers = api.GetTypes()
                .Where(t => t.Name.EndsWith("Controller") == false && t.Name.Contains("Controller"));

            foreach (var type in controllers)
                type.Name.ShouldEndWith("Controller", Case.Sensitive, $"{module}: {type.Name} must end with 'Controller'.");
        }

        // ---------------------------------------------------------------------
        // Services should end with "Service"
        // ---------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Services_ShouldBeSuffixed_WithService(string module)
        {
            var app = Assembly.Load($"{module}.Application");

            var services = app.GetTypes()
                .Where(t => t.IsClass && t.Name.Contains("Service"));

            foreach (var type in services)
                type.Name.ShouldEndWith("Service", Case.Sensitive, $"{module}: {type.Name} must end with 'Service'.");
        }

        // ---------------------------------------------------------------------
        // Repository implementations should end with "Repository"
        // ---------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Repositories_ShouldBeSuffixed_WithRepository(string module)
        {
            var infra = Assembly.Load($"{module}.Infrastructure");

            var repos = infra.GetTypes()
                .Where(t => t.IsClass && t.Name.Contains("Repository"));

            foreach (var type in repos)
                type.Name.ShouldEndWith("Repository", Case.Sensitive, $"{module}: {type.Name} must end with 'Repository'.");
        }

        // ---------------------------------------------------------------------
        // Interfaces must start with "I"
        // ---------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Interfaces_ShouldStartWith_I(string module)
        {
            var core = Assembly.Load($"{module}.Core");

            var interfaces = core.GetTypes().Where(t => t.IsInterface);

            foreach (var iface in interfaces)
                iface.Name.ShouldStartWith("I", Case.Sensitive, $"{module}: Interface {iface.Name} must start with 'I'.");
        }

        // ---------------------------------------------------------------------
        // CQRS Commands and CommandHandlers
        // ---------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Cqrs_Commands_ShouldHaveProperNaming(string module)
        {
            var app = Assembly.Load($"{module}.Application");

            var commands = app.GetTypes()
                .Where(t => t.Name.Contains("Command"));

            foreach (var cmd in commands)
                cmd.Name.ShouldEndWith("Command", Case.Sensitive, $"{module}: {cmd.Name} must end with 'Command'.");
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Cqrs_CommandHandlers_ShouldHaveProperNaming(string module)
        {
            var app = Assembly.Load($"{module}.Application");

            var handlers = app.GetTypes()
                .Where(t => t.Name.Contains("CommandHandler"));

            foreach (var handler in handlers)
                handler.Name.ShouldEndWith("CommandHandler", Case.Sensitive,$"{module}: {handler.Name} must end with 'CommandHandler'.");
        }

        // ---------------------------------------------------------------------
        // CQRS Queries and QueryHandlers
        // ---------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Cqrs_Queries_ShouldHaveProperNaming(string module)
        {
            var app = Assembly.Load($"{module}.Application");

            var queries = app.GetTypes()
                .Where(t => t.Name.Contains("Query"));

            foreach (var query in queries)
                query.Name.ShouldEndWith("Query", Case.Sensitive, $"{module}: {query.Name} must end with 'Query'.");
        }

        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void Cqrs_QueryHandlers_ShouldHaveProperNaming(string module)
        {
            var app = Assembly.Load($"{module}.Application");

            var handlers = app.GetTypes()
                .Where(t => t.Name.Contains("QueryHandler"));

            foreach (var handler in handlers)
                handler.Name.ShouldEndWith("QueryHandler", Case.Sensitive, $"{module}: {handler.Name} must end with 'QueryHandler'.");
        }

        // ---------------------------------------------------------------------
        // Domain Services must end with "DomainService"
        // ---------------------------------------------------------------------
        [Theory]
        [MemberData(nameof(ModuleNames))]
        public void DomainServices_ShouldBeSuffixed_WithDomainService(string module)
        {
            var core = Assembly.Load($"{module}.Core");

            var domainServices = core.GetTypes()
                .Where(t => t.IsClass && t.Namespace?.Contains(".DomainServices") == true);

            foreach (var service in domainServices)
                service.Name.ShouldEndWith("DomainService", Case.Sensitive, $"{module}: {service.Name} must end with 'DomainService'.");
        }
    }
}
