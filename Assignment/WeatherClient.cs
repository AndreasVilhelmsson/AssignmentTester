using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Assignment
{
	public class WeatherClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public WeatherClient(HttpClient httpClient, string apiKey)
		{
			_httpClient = httpClient;
			_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
		}

		public async Task<(double lat, double lon)> GetCoordinatesAsync(string cityName, string stateCode = "", string countryCode = "", int limit = 1)
		{
			if (string.IsNullOrWhiteSpace(cityName))
			{
				throw new ArgumentException("City name cannot be null or empty", nameof(cityName));
			}

			var requestUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName},{stateCode},{countryCode}&limit={limit}&appid={_apiKey}";
			var response = await _httpClient.GetAsync(requestUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Error fetching coordinates data.");
			}

			var content = await response.Content.ReadAsStringAsync();
			var json = JArray.Parse(content);

			if (json.Count == 0)
			{
				throw new Exception("No coordinates data found for the given city.");
			}

			var lat = json[0].Value<double>("lat");
			var lon = json[0].Value<double>("lon");

			return (lat, lon);
		}

		public async Task<(double temp, string description)> GetCurrentWeatherAsync(double lat, double lon)
		{
			var requestUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
			var response = await _httpClient.GetAsync(requestUrl);

			if (!response.IsSuccessStatusCode)
			{
				throw new HttpRequestException("Error fetching weather data.");
			}

			var content = await response.Content.ReadAsStringAsync();
			var json = JObject.Parse(content);

			var temp = json["main"].Value<double>("temp");
			var description = json["weather"][0].Value<string>("description");

			return (temp, description);
		}

		public async Task<(double temp, string description)> GetCurrentWeatherAsync(string cityName, string stateCode = "", string countryCode = "")
		{
			var (lat, lon) = await GetCoordinatesAsync(cityName, stateCode, countryCode);
			return await GetCurrentWeatherAsync(lat, lon);
		}
	}
}