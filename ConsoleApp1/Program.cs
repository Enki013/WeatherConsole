
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class WeatherData
{
    public required string temperature { get; set; }
    public required string wind { get; set; }
    public required string description { get; set; }
    public required Forecast[] forecast { get; set; }
}

class Forecast
{
    public required string day { get; set; }
    public required string temperature { get; set; }
    public required string wind { get; set; }
}

class Program
{
    static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        string[] cities = { "istanbul", "konya", "sivas" };

        foreach (string city in cities)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://goweather.herokuapp.com/weather/{city}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                WeatherData? weatherData = JsonSerializer.Deserialize<WeatherData>(responseBody);

                Console.WriteLine(city);
                Console.WriteLine("Bugün: " + weatherData?.temperature);
                Console.WriteLine("Rüzgar: " + weatherData?.wind);
                Console.WriteLine(weatherData?.description);
                Console.WriteLine("Tahmin");

                foreach (Forecast? forecast in weatherData?.forecast ?? Array.Empty<Forecast>())
                {
                    Console.WriteLine(forecast?.day + ". gün: " + forecast?.temperature + ", rüzgar: " + forecast?.wind);
                }

                Console.WriteLine();

            }
            catch (HttpRequestException)
            {
                Console.WriteLine($"Veri {city} için bulunamadı.");
            }
        }
    }
}