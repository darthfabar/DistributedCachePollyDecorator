using System.Runtime.CompilerServices;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedCachePollyDecorator.Policies
{
    /// <summary>
    /// Extensions to quickly add the decorator.
    /// </summary>
    public static class DepenencyInjectionExtension
    {

        /// <summary>
        /// Add the IDistributedCache decorator to DI and sets up the Policies for it.
        /// </summary>
        /// <typeparam name="TCircuitBreakerSettings">Settingstype.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="breakerSettings">Settings for policies.</param>
        /// <returns>IServiceCollection with injections.</returns>
        [Discardable]
        public static IServiceCollection AddDistributedCacheDecorator<TCircuitBreakerSettings>(this IServiceCollection services, TCircuitBreakerSettings breakerSettings)
            where TCircuitBreakerSettings : CircuitBreakerSettingsBase, new()
        {
            return services
                .AddPolicyForDistributedCache(breakerSettings)
                .Decorate<IDistributedCache, PolicyDistributedCacheDecorator>();
        }

        /// <summary>
        /// Add CacheDecorator with your own Polly Policies
        /// </summary>
        /// <typeparam name="TAsyncPolicy"></typeparam>
        /// <typeparam name="TSyncPolicy"></typeparam>
        /// <param name="services"></param>
        /// <param name="syncPolicy"></param>
        /// <param name="asyncPolicy"></param>
        /// <returns></returns>
        [Discardable]
        public static IServiceCollection AddDistributedCacheDecorator<TAsyncPolicy, TSyncPolicy>(this IServiceCollection services, TSyncPolicy syncPolicy, TAsyncPolicy asyncPolicy)
            where TAsyncPolicy : Polly.IAsyncPolicy
            where TSyncPolicy : Polly.ISyncPolicy
        {
            return services
                .AddPolicyForDistributedCache(syncPolicy, asyncPolicy)
                .Decorate<IDistributedCache, PolicyDistributedCacheDecorator>();
        }

        /// <summary>
        /// Creates a new PolicyRegistry and adds CircuitBreaker to it.
        /// </summary>
        /// <typeparam name="TCircuitBreakerSettings">Settingstype.</typeparam>
        /// <param name="services">Extension of services.</param>
        /// <param name="breakerSettings">Settings.</param>
        /// <returns>ÌServiceCollection.</returns>
        [Discardable]
        public static IServiceCollection AddPolicyForDistributedCache<TCircuitBreakerSettings>(this IServiceCollection services, TCircuitBreakerSettings breakerSettings)
            where TCircuitBreakerSettings : CircuitBreakerSettingsBase, new()
        {
            services
                .AddPolicyRegistry()
                .AddPoliciesToRegistry(PollyPolicies.GetSyncCircuitBreaker(breakerSettings), PollyPolicies.GetAsyncCircuitBreaker(breakerSettings));
            return services;
        }


        /// <summary>
        /// Register your own policies directly
        /// </summary>
        /// <typeparam name="TAsyncPolicy"></typeparam>
        /// <typeparam name="TSyncPolicy"></typeparam>
        /// <param name="services"></param>
        /// <param name="syncPolicy"></param>
        /// <param name="asyncPolicy"></param>
        /// <returns></returns>
        [Discardable]
        public static IServiceCollection AddPolicyForDistributedCache<TAsyncPolicy, TSyncPolicy>(this IServiceCollection services, TSyncPolicy syncPolicy, TAsyncPolicy asyncPolicy)
            where TAsyncPolicy : Polly.IAsyncPolicy
            where TSyncPolicy : Polly.ISyncPolicy
        {
            services
                .AddPolicyRegistry()
                .AddPoliciesToRegistry(syncPolicy, asyncPolicy);
            return services;
        }

        /// <summary>
        /// Adds Policies to a PolicyRegistry.
        /// </summary>
        /// <typeparam name="TAsyncPolicy"></typeparam>
        /// <typeparam name="TSyncPolicy"></typeparam>
        /// <param name="registry"></param>
        /// <param name="syncPolicy"></param>
        /// <param name="asyncPolicy"></param>
        /// <returns></returns>
        [Discardable]
        private static Polly.Registry.IPolicyRegistry<string> AddPoliciesToRegistry<TAsyncPolicy, TSyncPolicy>(this Polly.Registry.IPolicyRegistry<string> registry, TSyncPolicy syncPolicy, TAsyncPolicy asyncPolicy)
            where TAsyncPolicy : Polly.IAsyncPolicy
            where TSyncPolicy : Polly.ISyncPolicy
        {
            registry.Add(PollyPolicies.AsyncPolicy, asyncPolicy);
            registry.Add(PollyPolicies.SyncPolicy, syncPolicy);
            return registry;
        }
    }
}
