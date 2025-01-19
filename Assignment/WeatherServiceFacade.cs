using System.Threading.Tasks;

namespace Assignment
{
	public class WeatherServiceFacade
	{
		private readonly WeatherClient _weatherClient;

		public WeatherServiceFacade(WeatherClient weatherClient)
		{
			_weatherClient = weatherClient;
		}

		public async Task<string> GetWeather(string city)
		{
			var (temp, description) = await _weatherClient.GetCurrentWeatherAsync(city);
			return $"The temperature in {city} is {temp}°C with {description}.";
		}
	}
}