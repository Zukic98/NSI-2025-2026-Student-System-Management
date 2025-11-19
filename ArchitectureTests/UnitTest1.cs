using System;
using Xunit;

namespace ArchitectureTests
{
    public class UnitTest1
    {
        [Fact]
        public void SampleServicesExistTest()
        {
            // Provjera da li klase postoje
            Assert.NotNull(typeof(Notifications.Application.Services.SampleService));
            Assert.NotNull(typeof(Analytics.Application.Services.SampleService));
            Assert.NotNull(typeof(Faculty.Application.Services.SampleService));
        }

        [Fact]
        public void SampleServicesNamespaceTest()
        {
            // Provjera da li su u pravim namespace-ima
            Assert.Equal("Notifications.Application.Services", typeof(Notifications.Application.Services.SampleService).Namespace);
            Assert.Equal("Analytics.Application.Services", typeof(Analytics.Application.Services.SampleService).Namespace);
            Assert.Equal("Faculty.Application.Services", typeof(Faculty.Application.Services.SampleService).Namespace);
        }
    }
}
