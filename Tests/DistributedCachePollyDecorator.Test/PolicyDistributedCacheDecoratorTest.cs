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
        public async Task Circuit_Should_Open_After_3_GetAync_Tries_Async()
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

        [Fact]
        public void Circuit_Should_Open_After_3_Get_Tries()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupGetWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            Func<byte[]> callGet = () => distributedCache.Get("cachekey");
            for (var i = 0; i < 2; i++)
            {
                _ = callGet.Should().Throw<SocketException>();
            }
            callGet.Should().Throw<BrokenCircuitException>();
        }

        [Fact]
        public async Task Circuit_Should_Open_After_3_SetAync_Tries_Async()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupSetAsyncWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();
            var dataForCache = new byte[] { 123, 213, 123 };

            Func<Task> callSetAsync = () => distributedCache.SetAsync("cachekey", dataForCache, CancellationToken.None);
            for (var i = 0; i < 2; i++)
            {
                _ = await callSetAsync.Should().ThrowAsync<SocketException>().ConfigureAwait(false);
            }
            await callSetAsync.Should().ThrowAsync<BrokenCircuitException>().ConfigureAwait(false);
        }

        [Fact]
        public void Circuit_Should_Open_After_3_Set_Tries()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupSetWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();
            var dataForCache = new byte[] { 123, 213, 123 };

            Action callSet = () => distributedCache.Set("cachekey", dataForCache);
            for (var i = 0; i < 2; i++)
            {
                _ = callSet.Should().Throw<SocketException>();
            }
            callSet.Should().Throw<BrokenCircuitException>();
        }

        [Fact]
        public async Task Circuit_Should_Open_After_3_RemoveAync_Tries_Async()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupRemoveAsyncWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();
            Func<Task> callRemoveAsync = () => distributedCache.RemoveAsync("cachekey", CancellationToken.None);
            for (var i = 0; i < 2; i++)
            {
                _ = await callRemoveAsync.Should().ThrowAsync<SocketException>().ConfigureAwait(false);
            }
            await callRemoveAsync.Should().ThrowAsync<BrokenCircuitException>().ConfigureAwait(false);
        }

        [Fact]
        public void Circuit_Should_Open_After_3_Remove_Tries()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupRemoveWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            Action callRemove = () => distributedCache.Remove("cachekey");
            for (var i = 0; i < 2; i++)
            {
                _ = callRemove.Should().Throw<SocketException>();
            }
            callRemove.Should().Throw<BrokenCircuitException>();
        }

        [Fact]
        public async Task Circuit_Should_Open_After_3_RefreshAync_Tries_Async()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupRefreshAsyncWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();
            Func<Task> callRefreshAsync = () => distributedCache.RefreshAsync("cachekey", CancellationToken.None);
            for (var i = 0; i < 2; i++)
            {
                _ = await callRefreshAsync.Should().ThrowAsync<SocketException>().ConfigureAwait(false);
            }
            await callRefreshAsync.Should().ThrowAsync<BrokenCircuitException>().ConfigureAwait(false);
        }

        [Fact]
        public void Circuit_Should_Open_After_3_Refresh_Tries()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>();
            cacheMock.SetupRefreshWithException(new SocketException());
            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            Action callRefresh = () => distributedCache.Refresh("cachekey");
            for (var i = 0; i < 2; i++)
            {
                _ = callRefresh.Should().Throw<SocketException>();
            }
            callRefresh.Should().Throw<BrokenCircuitException>();
        }

        [Fact]
        public void Get_Should_Call_Inner_Get()
        {
            var dataForCache = new byte[] { 123, 213, 123 };
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupGetWithSuccess(dataForCache);

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            var data = distributedCache.Get("key");

            data.Should().BeEquivalentTo(dataForCache);

            cacheMock.Verify(v => v.Get(It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task GetAsync_Should_Call_Inner_GetAsync()
        {
            var dataForCache = new byte[] { 123, 213, 123 };
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupGetAsyncWithSuccess(dataForCache);

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            var data = await distributedCache.GetAsync("key").ConfigureAwait(true);

            data.Should().BeEquivalentTo(dataForCache);

            cacheMock.Verify(v => v.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public void Set_Should_Call_Inner_Set()
        {
            var dataForCache = new byte[] { 123, 213, 123 };
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupSetWithSuccess();

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            distributedCache.Set("key", dataForCache);

           cacheMock.Verify(v => v.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }


        [Fact]
        public async Task SetAsync_Should_Call_Inner_SetAsync()
        {
            var dataForCache = new byte[] { 123, 213, 123 };
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupSetAsyncWithSuccess();

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            await distributedCache.SetAsync("key", dataForCache).ConfigureAwait(true);

            cacheMock.Verify(v => v.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Remvoe_Should_Call_Inner_Remove()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupRemoveWithSuccess();

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            distributedCache.Remove("key");

            cacheMock.Verify(v => v.Remove(It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task RemoveAsync_Should_Call_Inner_RemoveAsync()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupRemoveAsyncWithSuccess();

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            await distributedCache.RemoveAsync("key").ConfigureAwait(true);


            cacheMock.Verify(v => v.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Remvoe_Should_Call_Inner_Refresh()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupRefreshWithSuccess();

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            distributedCache.Refresh("key");

            cacheMock.Verify(v => v.Refresh(It.IsAny<string>()), Times.Once);
        }


        [Fact]
        public async Task RefreshAsync_Should_Call_Inner_RefreshAsync()
        {
            var sb = GetServiceProvider();
            var cacheMock = sb.GetRequiredService<Mock<IDistributedCache>>()
                .SetupRefreshAsyncWithSuccess();

            var distributedCache = sb.GetRequiredService<IDistributedCache>();

            await distributedCache.RefreshAsync("key").ConfigureAwait(true);


            cacheMock.Verify(v => v.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
