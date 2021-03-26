using DistributedCachePollyDecorator.Policies;
using Microsoft.Extensions.Caching.Distributed;
using Polly;
using Polly.Registry;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedCachePollyDecorator
{
    /// <summary>
    /// Decorator Layer for IDistributedCache Interface.
    /// </summary>
    public class PolicyDistributedCacheDecorator : IDistributedCache
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ISyncPolicy _syncPolicy;
        private readonly IAsyncPolicy _asyncPolicy;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="distributedCache"></param>
        public PolicyDistributedCacheDecorator(IReadOnlyPolicyRegistry<string> registry, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _syncPolicy = registry.Get<ISyncPolicy>(PollyPolicies.SyncPolicy);
            _asyncPolicy = registry.Get<IAsyncPolicy>(PollyPolicies.AsyncPolicy);
        }

        /// <summary>
        /// Sync Get 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] Get(string key)
        {
            return _syncPolicy.Execute(() => _distributedCache.Get(key));
        }

        /// <summary>
        /// Async Get
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            return await _asyncPolicy.ExecuteAsync(() => _distributedCache.GetAsync(key, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Refresh(string key)
        {
            _syncPolicy.Execute(() => _distributedCache.Refresh(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            await _asyncPolicy.ExecuteAsync(() => _distributedCache.RefreshAsync(key, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _syncPolicy.Execute(() => _distributedCache.Remove(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await _asyncPolicy.ExecuteAsync(() => _distributedCache.RemoveAsync(key, token)).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _syncPolicy.Execute(() => _distributedCache.Remove(key));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            await _asyncPolicy.ExecuteAsync(() => _distributedCache.RemoveAsync(key, token)).ConfigureAwait(false);
        }
    }
}
