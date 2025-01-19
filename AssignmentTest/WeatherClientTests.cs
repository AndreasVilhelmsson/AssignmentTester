using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Assignment.Tests
{
	public class WeatherClientTests
	{
		private readonly string _apiKey = "dummy_api_key";

		[Fact]
		public async Task GetCoordinatesAsync_ReturnsCorrectCoordinates()
		{
			// Arrange
			var cityName = "London";
			var expectedLat = 51.5074;
			var expectedLon = -0.1278;
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent($"[{{\"lat\": {expectedLat}, \"lon\": {expectedLon}}}]")
				});

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act
			var (lat, lon) = await weatherClient.GetCoordinatesAsync(cityName);

			// Assert
			Assert.Equal(expectedLat, lat);
			Assert.Equal(expectedLon, lon);
		}

		[Fact]
		public async Task GetCoordinatesAsync_ThrowsHttpRequestExceptionOnNetworkError()
		{
			// Arrange
			var cityName = "London";
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ThrowsAsync(new HttpRequestException());

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => weatherClient.GetCoordinatesAsync(cityName));
		}

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public async Task GetCoordinatesAsync_ThrowsArgumentExceptionOnInvalidCityName(string cityName)
		{
			// Arrange
			var httpClient = new HttpClient();
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act & Assert
			await Assert.ThrowsAsync<ArgumentException>(() => weatherClient.GetCoordinatesAsync(cityName));
		}

		[Fact]
		public async Task GetCoordinatesAsync_ThrowsExceptionWhenNoDataFound()
		{
			// Arrange
			var cityName = "InvalidCity";
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent("[]")
				});

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(() => weatherClient.GetCoordinatesAsync(cityName));
		}

		[Fact]
		public async Task GetCurrentWeatherAsync_ReturnsCorrectWeather()
		{
			// Arrange
			var lat = 51.5074;
			var lon = -0.1278;
			var expectedTemp = 12.34; // Example temperature
			var expectedDescription = "clear sky";
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent($"{{\"main\": {{\"temp\": {expectedTemp}}}, \"weather\": [{{\"description\": \"{expectedDescription}\"}}]}}")
				});

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act
			var (temp, description) = await weatherClient.GetCurrentWeatherAsync(lat, lon);

			// Assert
			Assert.Equal(expectedTemp, temp);
			Assert.Equal(expectedDescription, description);
		}

		[Fact]
		public async Task GetCurrentWeatherAsync_ThrowsHttpRequestExceptionOnNetworkError()
		{
			// Arrange
			var lat = 51.5074;
			var lon = -0.1278;
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ThrowsAsync(new HttpRequestException());

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => weatherClient.GetCurrentWeatherAsync(lat, lon));
		}

		// Additional crash scenarios
		[Fact]
		public async Task GetCurrentWeatherAsync_ThrowsExceptionOnInvalidLatLon()
		{
			// Arrange
			var lat = -999.0; // Invalid latitude
			var lon = -999.0; // Invalid longitude
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.BadRequest
				});

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => weatherClient.GetCurrentWeatherAsync(lat, lon));
		}

		[Fact]
		public async Task GetCoordinatesAsync_ThrowsExceptionOnInvalidApiKey()
		{
			// Arrange
			var cityName = "London";
			var invalidApiKey = "invalid_api_key";
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.Unauthorized
				});

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, invalidApiKey);

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => weatherClient.GetCoordinatesAsync(cityName));
		}

		[Fact]
		public async Task GetCurrentWeatherAsync_ThrowsExceptionOnServerError()
		{
			// Arrange
			var lat = 51.5074;
			var lon = -0.1278;
			var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
			mockHttpMessageHandler.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.InternalServerError
				});

			var httpClient = new HttpClient(mockHttpMessageHandler.Object);
			var weatherClient = new WeatherClient(httpClient, _apiKey);

			// Act & Assert
			await Assert.ThrowsAsync<HttpRequestException>(() => weatherClient.GetCurrentWeatherAsync(lat, lon));
		}
	}
}