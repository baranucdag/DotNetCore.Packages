using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetCore.Packages.Application.Common.Services.Cache;
using DotNetCore.Packages.Application.Common.Shared.ResultTypes;
using StackExchange.Redis;
namespace DotNetCore.Packages.Infrastructure.Services.Cache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;
    private readonly string _instanceName;

    public RedisCacheService(string connectionString, string instanceName)
    {
        var redis = ConnectionMultiplexer.Connect(connectionString);
        _database = redis.GetDatabase();
        _instanceName = instanceName;
    }

    public async Task SetJsonAsync<T>(string key, T data, TimeSpan expiration)
    {
        var options = new JsonSerializerOptions
            { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
        var jsonData = JsonSerializer.Serialize(data, options);
        await _database.StringSetAsync($"{_instanceName}{key}", jsonData, expiration);
    }

    public async Task<T?> GetJsonAsync<T>(string key)
    {
        var value = await _database.StringGetAsync($"{_instanceName}{key}");
        if (value.IsNullOrEmpty) return default;

        try
        {
            var jsonDoc = JsonDocument.Parse(value.ToString());
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("Type", out var typeProperty) && root.TryGetProperty("Data", out var dataProperty))
            {
                var type = Type.GetType(typeProperty.GetString()!);
                if (type == null) return default;

                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter(), new IDataResultConverter<T>() },
                    PropertyNameCaseInsensitive = true
                };

                return (T?)JsonSerializer.Deserialize(dataProperty.GetRawText(), type, options);
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[CACHE] JSON Deserialize Hatası: {ex.Message} | Key: {key}");
        }

        return default;
    }


    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync($"{_instanceName}{key}");
    }
}

public class IDataResultConverter<T> : JsonConverter<IDataResult<T>>
{
    public override IDataResult<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
        if (jsonObject.TryGetProperty("Success", out var successProperty) && successProperty.GetBoolean())
        {
            return JsonSerializer.Deserialize<SuccessDataResult<T>>(jsonObject.GetRawText(), options);
        }
        else
        {
            return JsonSerializer.Deserialize<ErrorDataResult<T>>(jsonObject.GetRawText(), options);
        }
    }

    public override void Write(Utf8JsonWriter writer, IDataResult<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}