using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedCachePollyDecorator.Tests
{
    public static class DistributedCacheMock
    {
        public static Mock<IDistributedCache> GetMock()
        {
            return new Mock<IDistributedCache>();
        }

        public static Mock<IDistributedCache> SetupGetAsyncWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            return mock;
        }

        public static Mock<IDistributedCache> SetupGetWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.Get(It.IsAny<string>())).Throws(exception);

            return mock;
        }

        public static Mock<IDistributedCache> SetupSetAsyncWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            return mock;
        }

        public static Mock<IDistributedCache> SetupSetWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>())).Throws(exception);

            return mock;
        }


        public static Mock<IDistributedCache> SetupRemoveAsyncWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            return mock;
        }

        public static Mock<IDistributedCache> SetupRemoveWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.Remove(It.IsAny<string>())).Throws(exception);

            return mock;
        }

        public static Mock<IDistributedCache> SetupRefreshAsyncWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            return mock;
        }

        public static Mock<IDistributedCache> SetupRefreshWithException(this Mock<IDistributedCache> mock, Exception exception)
        {
            mock.Setup(s => s.Refresh(It.IsAny<string>())).Throws(exception);

            return mock;
        }


        public static Mock<IDistributedCache> SetupGetAsyncWithSuccess(this Mock<IDistributedCache> mock, byte[] data)
        {
            mock.Setup(s => s.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(data);

            return mock;
        }

        public static Mock<IDistributedCache> SetupGetWithSuccess(this Mock<IDistributedCache> mock, byte[] data)
        {
            mock.Setup(s => s.Get(It.IsAny<string>())).Returns(data);

            return mock;
        }

        public static Mock<IDistributedCache> SetupSetAsyncWithSuccess(this Mock<IDistributedCache> mock)
        {
            mock.Setup(s => s.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            return mock;
        }

        public static Mock<IDistributedCache> SetupSetWithSuccess(this Mock<IDistributedCache> mock)
        {
            mock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()));

            return mock;
        }


        public static Mock<IDistributedCache> SetupRemoveAsyncWithSuccess(this Mock<IDistributedCache> mock)
        {
            mock.Setup(s => s.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            return mock;
        }

        public static Mock<IDistributedCache> SetupRemoveWithSuccess(this Mock<IDistributedCache> mock)
        {
            mock.Setup(s => s.Remove(It.IsAny<string>()));

            return mock;
        }

        public static Mock<IDistributedCache> SetupRefreshAsyncWithSuccess(this Mock<IDistributedCache> mock)
        {
            mock.Setup(s => s.RefreshAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            return mock;
        }

        public static Mock<IDistributedCache> SetupRefreshWithSuccess(this Mock<IDistributedCache> mock)
        {
            mock.Setup(s => s.Refresh(It.IsAny<string>()));

            return mock;
        }

        public static IServiceCollection AddDistributedCacheMock(this IServiceCollection services)
        {
            var mock = GetMock();
            services.AddSingleton(mock);
            services.AddSingleton(provider => provider.GetRequiredService<Mock<IDistributedCache>>().Object);
            return services;
        }
    }
}
