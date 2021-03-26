using System;

namespace DistributedCachePollyDecorator.Policies
{
    /// <summary>
    /// Abstract class.
    /// </summary>
    public abstract class CircuitBreakerSettingsBase
    {

    }

    /// <summary>
    /// Settings Class for a circuitbreaker
    /// </summary>
    public class CircuitBreakerSettings : CircuitBreakerSettingsBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan DurationOfBreak { get; set; }
    }

    /// <summary>
    /// Settings for an advanced Circuit Breaker
    /// </summary>
    public class CircuitBreakerAdvancedSettings : CircuitBreakerSettingsBase
    {
        /// <summary>
        /// 
        /// </summary>
        public double FailureThreshold { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan SamplingDuration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int MinimumThroughput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan DurationOfBreak { get; set; }
    }
}
