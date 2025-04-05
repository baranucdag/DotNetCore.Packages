using DotNetCore.Packages.Application.Common.Services.Cache;
using StackExchange.Redis;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace DotNetCore.Packages.Infrastructure.Services.Cache;

public class RedisLockService: IRedisLockService
{
    private readonly RedLockFactory _redLockFactory;

    public RedisLockService(string connectionString)
    {
        var redisMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        _redLockFactory = RedLockFactory.Create(new List<RedLockMultiplexer> { redisMultiplexer });
    }

    /// <summary>
    /// Belirtilen anahtar için distributed lock alır ve gerçek sonucu döndürür.
    /// </summary>
    public async Task<TResponse> AcquireLockAsync<TResponse>(string resource, TimeSpan expiry, Func<Task<TResponse>> action)
    {
        using (var redLock = await _redLockFactory.CreateLockAsync(resource, expiry))
        {
            if (!redLock.IsAcquired) throw new Exception($"Lock alınamadı: {resource}");

            return await action();
        }
    }
}