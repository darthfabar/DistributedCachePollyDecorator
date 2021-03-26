using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading;

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

        public static IServiceCollection AddDistributedCacheMock(this IServiceCollection services)
        {
            var mock = GetMock();
            services.AddSingleton(mock);
            services.AddSingleton(provider => provider.GetRequiredService<Mock<IDistributedCache>>().Object);
            return services;
        }
    }
}
