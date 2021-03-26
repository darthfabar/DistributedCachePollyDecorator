# DistributedCachePollyDecorator

[![GitHub Actions Status](https://github.com/darthfabar/DistributedCachePollyDecorator/workflows/Build/badge.svg?branch=main)](https://github.com/darthfabar/DistributedCachePollyDecorator/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/darthfabar/DistributedCachePollyDecorator?branch=main&includeBuildsFromPullRequest=false)](https://github.com/darthfabar/DistributedCachePollyDecorator/actions)

# What Does This Package Do?
With this package you can inject Polly policy behaviours to the IDistributedCache interface. 
Right now it enables you to have a circuitbreaker policy that enables you to fail fast in case you Redis is down or your app runs into connection errors.


#How to use the package:

Just add a 'AddDistributedCacheDecorator' call in your Startup.cs / any other place where you do your registrations
```cs
	public void ConfigureServices(IServiceCollection services)
	{

		services.AddControllers();

		// your settings for Redis
		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = "localhost:6379,allowAdmin=true,DefaultDatabase=1";
		});

		// Decorate the inferface with a simple circuit breaker
		services.AddDistributedCacheDecorator(new CircuitBreakerSettings()
		{
			DurationOfBreak = TimeSpan.FromMinutes(5),
			ExceptionsAllowedBeforeBreaking = 2
		});

		// OR decorate it with an advanced one: (don't add both)
		services.AddDistributedCacheDecorator(new CircuitBreakerAdvancedSettings()
		{
			DurationOfBreak = TimeSpan.FromMinutes(5),
			FailureThreshold = 0.5,
			MinimumThroughput = 100,
			SamplingDuration = TimeSpan.FromMinutes(2)
		});
	}
```
