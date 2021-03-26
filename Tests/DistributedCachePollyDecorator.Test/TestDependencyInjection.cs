using DistributedCachePollyDecorator.Policies;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace DistributedCachePollyDecorator.Tests
{

    public class TestDependencyInjection
    {

        [Fact]
        public void Test1()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDistributedCacheMock();
            serviceCollection.AddDistributedCacheDecorator(new CircuitBreakerSettings() { ExceptionsAllowedBeforeBreaking = 2, DurationOfBreak = TimeSpan.FromMinutes(2) });

            var sb = serviceCollection.BuildServiceProvider();


            var distributedCache = sb.GetRequiredService<IDistributedCache>();
            distributedCache.Should().NotBeNull();
            distributedCache.Should().BeOfType<PolicyDistributedCacheDecorator>();

        }
    }
}
