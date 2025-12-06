using Disc.NET.Shared.Configurations;
using System;
using System.Linq;
using System.Text.Json;

namespace Disc.NET.Shared.Serializer
{
    internal sealed class DiscNetSerializer
    {
        private static DiscNetSerializer? _instance;

        private readonly JsonSerializerOptions _options;

        private DiscNetSerializer()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                PropertyNameCaseInsensitive = true
            };
        }

        public static DiscNetSerializer GetInstance()
        {
            if (_instance == null)
                _instance = new DiscNetSerializer();

            return _instance;
        }


        public T? Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        public T? Deserialize<T>(JsonElement json)
        {
            var jsonString = JsonSerializer.Serialize(json.Deserialize<JsonDocument>());
            return Deserialize<T>(jsonString);
        }




        public async Task<T?> DeserializeAsync<T>(Stream json, CancellationToken cancellation = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(json, _options, cancellation)
                .ConfigureAwait(false);
        }

        public string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }
    }


    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            var result = new System.Text.StringBuilder();

            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (char.IsUpper(c))
                {
                    if (i > 0)
                        result.Append('_');

                    result.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }

}