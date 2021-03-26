using DistributedCachePollyDecorator.Policies;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Polly.CircuitBreaker;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DistributedCachePollyDecorator.Tests
{

    public class PolicyDistributedCacheDecoratorTest
    {

        private static IServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDistributedCacheMock();
            serviceCollection.AddDistributedCacheDecorator(new CircuitBreakerSettings() { ExceptionsAllowedBeforeBreaking = 2, DurationOfBreak = TimeSpan.FromMinutes(2) });

            return serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task Circuit_Should_Open_After_GetAync_3_Tries_Async()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupGetAsyncWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            Func<Task> callGetAsync = () => distributedCache.GetAsync("cachekey", CancellationToken.None);
            for (var i = 0; i < 2; i++)
            {
                _ = await callGetAsync.Should().ThrowAsync<SocketException>().ConfigureAwait(false);
            }
            await callGetAsync.Should().ThrowAsync<BrokenCircuitException>().ConfigureAwait(false);
        }
    }
}
