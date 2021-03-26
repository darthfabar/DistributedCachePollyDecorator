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
                .AddPoliciesToRegistry(breakerSettings);
            return services;
        }

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
        /// Adds Policies to a PolicyRegistry.
        /// </summary>
        /// <typeparam name="TCircuitBreakerSettings">Settingstype.</typeparam>
        /// <param name="registry">PolicyRegistry to use.</param>
        /// <param name="breakerSettings">Settings.</param>
        /// <returns>same PolicyRegistry instance as before.</returns>
        [Discardable]
        private static Polly.Registry.IPolicyRegistry<string> AddPoliciesToRegistry<TCircuitBreakerSettings>(this Polly.Registry.IPolicyRegistry<string> registry, TCircuitBreakerSettings breakerSettings)
            where TCircuitBreakerSettings : CircuitBreakerSettingsBase, new()
        {
            registry.Add(PollyPolicies.AsyncPolicy, PollyPolicies.GetAsyncCircuitBreaker(breakerSettings));
            registry.Add(PollyPolicies.SyncPolicy, PollyPolicies.GetSyncCircuitBreaker(breakerSettings));
            return registry;
        }
    }
}
