# DistributedCachePollyDecorator

[![GitHub Actions Status](https://github.com/darthfabar/DistributedCachePollyDecorator/workflows/Build/badge.svg?branch=main)](https://github.com/darthfabar/DistributedCachePollyDecorator/actions)

[![GitHub Actions Build History](https://buildstats.info/github/chart/darthfabar/DistributedCachePollyDecorator?branch=main&includeBuildsFromPullRequest=false)](https://github.com/darthfabar/DistributedCachePollyDecorator/actions)

# What Does This Package Do?
With this package you can inject Polly policy behaviours to the IDistributedCache interface. 
Right now it enables you to have a circuitbreaker policy that enables you to fail fast in case you Redis is down or your app runs into connection errors.

