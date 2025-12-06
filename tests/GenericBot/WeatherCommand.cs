using Disc.NET.Client.SDK.Messages;
using Disc.NET.Client.SDK.Messages.Embeds;
using Disc.NET.Commands;
using Disc.NET.Commands.Attributes;
using Disc.NET.Commands.Contexts;
using Disc.NET.Shared.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenericBot
{
    [SlashCommand("tempo", InteractionType.SubCommand, "Ver as informações de tempo da sua cidade")]
    public class WeatherCommand : ISlashCommand
    {
        private readonly HttpClient _httpClient;

        public WeatherCommand(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> RunAsync(InteractionContext context, SlashCommandParamsResult @params)
        {

            var weatherApi =
                "https://api.open-meteo.com/v1/forecast?latitude=52.52&longitude=13.41&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m";

            var response = await _httpClient.GetAsync(weatherApi);

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();
            var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(content);

            var current = weatherResponse?.Current;
            var units = weatherResponse?.CurrentUnits;

            var message = new ApiMessage()
            {
                MessageFlags = new()
                {
                    MessageFlag.Ephemeral
                },
                Embeds = new List<Embed>()
                {
                    new Embed()
                    {
                        Title = "🌤️ Clima Atual",
                        Description = "Previsão atualizada para a região solicitada.",
                        Color = 0x4AA8FF,
                        Timestamp = DateTime.UtcNow.ToString("o"),

                        Thumbnail = new EmbedImage
                        {
                            Url = "https://i.imgur.com/8fK4h6X.png"
                        },

                        Author = new EmbedAuthor
                        {
                            Name = "Open-Meteo API",
                            Url = "https://open-meteo.com"
                        },

                        Fields = new List<EmbedField>
                        {
                            new EmbedField
                            {
                                Name = "🌡️ Temperatura",
                                Value = $"{current?.Temperature2m} {units?.Temperature2m}",
                                Inline = true
                            },
                            new EmbedField
                            {
                                Name = "💨 Vento",
                                Value = $"{current?.WindSpeed10m} {units?.WindSpeed10m}",
                                Inline = true
                            },
                            new EmbedField
                            {
                                Name = "📍 Localização",
                                Value = $"Lat: **{weatherResponse?.Latitude}**, Lon: **{weatherResponse?.Longitude}**",
                                Inline = false
                            },
                            new EmbedField
                            {
                                Name = "🕒 Timezone",
                                Value = $"{weatherResponse?.Timezone} ({weatherResponse?.TimezoneAbbreviation})",
                                Inline = true
                            },
                            new EmbedField
                            {
                                Name = "⏱️ Intervalo",
                                Value = $"{current?.Interval} segundos",
                                Inline = true
                            },
                            new EmbedField
                            {
                                Name = "🗓️ Última atualização",
                                Value = current?.Time ?? "N/A",
                                Inline = false
                            }
                        },

                        Footer = new EmbedFooter
                        {
                            Text = "Dados meteorológicos fornecidos por Open-Meteo",
                            IconUrl = "https://i.imgur.com/Qp9ZQpD.png"
                        }
                    }
                }
            };

            //var channelId = context.Channel?.Id;

            //if (!string.IsNullOrEmpty(channelId))
            //    await UseClient().SendMessageAsync(channelId, message);

            await context.Response.SendMessageAsync(message, CancellationToken.None);

            return true;
        }
    }

    public class WeatherResponse
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double GenerationTimeMs { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("timezone_abbreviation")]
        public string TimezoneAbbreviation { get; set; }

        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }

        [JsonPropertyName("current_units")]
        public CurrentUnits CurrentUnits { get; set; }

        [JsonPropertyName("current")]
        public Current Current { get; set; }

        [JsonPropertyName("hourly_units")]
        public HourlyUnits HourlyUnits { get; set; }

        [JsonPropertyName("hourly")]
        public Hourly Hourly { get; set; }
    }

    public class CurrentUnits
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("interval")]
        public string Interval { get; set; }

        [JsonPropertyName("temperature_2m")]
        public string Temperature2m { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public string WindSpeed10m { get; set; }
    }

    public class Current
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("interval")]
        public int Interval { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double Temperature2m { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed10m { get; set; }
    }

    public class HourlyUnits
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public string Temperature2m { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public string RelativeHumidity2m { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public string WindSpeed10m { get; set; }
    }

    public class Hourly
    {
        [JsonPropertyName("time")]
        public List<string> Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public List<double> Temperature2m { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public List<int> RelativeHumidity2m { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public List<double> WindSpeed10m { get; set; }
    }
}
