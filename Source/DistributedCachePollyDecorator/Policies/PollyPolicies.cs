using Polly;
using Polly.CircuitBreaker;
using System;
using System.IO;
using System.Net.Sockets;

namespace DistributedCachePollyDecorator.Policies
{
    /// <summary>
    /// Helper to create predefined Polly Policies.
    /// </summary>
    public class PollyPolicies
    {
        /// <summary>
        /// Sync Policy Name in PolicyRegestry.
        /// </summary>
        public const string SyncPolicy = "SyncPolicy";

        /// <summary>
        /// ASync Policy Name in PolicyRegestry.
        /// </summary>
        public const string AsyncPolicy = "AsyncPolicy";

        /// <summary>
        /// Creates Async CircuitBreakerPolicy.
        /// </summary>
        /// <typeparam name="TCircuitBreakerSettings">Settingstype.</typeparam>
        /// <param name="circuitBreakerSettings">Settings for Circuit Breaker.</param>
        /// <returns>Policy.</returns>
        public static AsyncCircuitBreakerPolicy GetAsyncCircuitBreaker<TCircuitBreakerSettings>(TCircuitBreakerSettings circuitBreakerSettings)
            where TCircuitBreakerSettings : CircuitBreakerSettingsBase, new()
        {
            if (circuitBreakerSettings is CircuitBreakerSettings settings)
            {
                return GetAsyncCircuitBreaker(settings);
            }

            return circuitBreakerSettings is CircuitBreakerAdvancedSettings settingsAdvanced
                ? GetAsyncCircuitBreaker(settingsAdvanced)
                : GetAsyncCircuitBreaker(new CircuitBreakerSettings() { DurationOfBreak = TimeSpan.FromMinutes(5), ExceptionsAllowedBeforeBreaking = 10 });
        }

        /// <summary>
        /// Creates Sync CircuitBreakerPolicy.
        /// </summary>
        /// <typeparam name="TCircuitBreakerSettings">Settingstype.</typeparam>
        /// <param name="circuitBreakerSettings">Settings for Circuit Breaker.</param>
        /// <returns>Policy.</returns>
        public static CircuitBreakerPolicy GetSyncCircuitBreaker<TCircuitBreakerSettings>(TCircuitBreakerSettings circuitBreakerSettings)
             where TCircuitBreakerSettings : CircuitBreakerSettingsBase, new()
        {
            if (circuitBreakerSettings is CircuitBreakerSettings settings)
            {
                return GetSyncCircuitBreaker(settings);
            }

            return circuitBreakerSettings is CircuitBreakerAdvancedSettings settingsAdvanced
                ? GetSyncCircuitBreaker(settingsAdvanced)
                : GetSyncCircuitBreaker(new CircuitBreakerSettings() { DurationOfBreak = TimeSpan.FromMinutes(5), ExceptionsAllowedBeforeBreaking = 10 });
        }

        /// <summary>
        /// Creates Async CircuitBreakerPolicy.
        /// </summary>
        /// <param name="circuitBreakerSettings">Settings for Circuit Breaker.</param>
        /// <returns>Policy.</returns>
        public static AsyncCircuitBreakerPolicy GetAsyncCircuitBreaker(CircuitBreakerSettings circuitBreakerSettings)
        {
            var breaker = BasePolicyBuilder()
                .CircuitBreakerAsync(circuitBreakerSettings.ExceptionsAllowedBeforeBreaking, circuitBreakerSettings.DurationOfBreak);

            return breaker;
        }

        /// <summary>
        /// Creates Async CircuitBreakerPolicy.
        /// </summary>
        /// <param name="circuitBreakerSettings">Settings for Circuit Breaker.</param>
        /// <returns>Policy.</returns>
        public static AsyncCircuitBreakerPolicy GetAsyncCircuitBreaker(CircuitBreakerAdvancedSettings circuitBreakerSettings)
        {
            var breaker = BasePolicyBuilder()
                .AdvancedCircuitBreakerAsync(
                    circuitBreakerSettings.FailureThreshold,
                    circuitBreakerSettings.SamplingDuration,
                    circuitBreakerSettings.MinimumThroughput,
                    circuitBreakerSettings.DurationOfBreak);

            return breaker;
        }

        /// <summary>
        /// Creates sync CircuitBreakerPolicy.
        /// </summary>
        /// <param name="circuitBreakerSettings">Settings for Circuit Breaker.</param>
        /// <returns>Policy.</returns>
        public static CircuitBreakerPolicy GetSyncCircuitBreaker(CircuitBreakerAdvancedSettings circuitBreakerSettings)
        {
            var breaker = BasePolicyBuilder()
                .AdvancedCircuitBreaker(
                    circuitBreakerSettings.FailureThreshold,
                    circuitBreakerSettings.SamplingDuration,
                    circuitBreakerSettings.MinimumThroughput,
                    circuitBreakerSettings.DurationOfBreak);

            return breaker;
        }

        /// <summary>
        /// Creates sync CircuitBreakerPolicy.
        /// </summary>
        /// <param name="circuitBreakerSettings">Settings for Circuit Breaker.</param>
        /// <returns>Policy.</returns>
        public static CircuitBreakerPolicy GetSyncCircuitBreaker(CircuitBreakerSettings circuitBreakerSettings)
        {
            var breaker = BasePolicyBuilder()
                .CircuitBreaker(circuitBreakerSettings.ExceptionsAllowedBeforeBreaking, circuitBreakerSettings.DurationOfBreak);

            return breaker;
        }

        private static PolicyBuilder BasePolicyBuilder()
        {
            return Policy.Handle<SocketException>();
        }
    }

}
