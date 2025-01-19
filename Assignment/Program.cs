using System;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetEnv;

namespace Assignment
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Env.Load();

			var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");

			if (string.IsNullOrEmpty(apiKey))
			{
				Console.WriteLine("API key is missing. Please set the OPENWEATHER_API_KEY environment variable.");
				return;
			}

			var httpClient = new HttpClient();
			var weatherClient = new WeatherClient(httpClient, apiKey);
			var weatherServiceFacade = new WeatherServiceFacade(weatherClient);

			try
			{
				// WeatherServiceFacade example gothenburg, filter out temp and description
				string weather = await weatherServiceFacade.GetWeather("Gothenburg");
				Console.WriteLine($"\nWeather in Gothenburg: {weather}\n");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}

			// uppgift kalulator
			Console.WriteLine($"Add: {Calculator.Add(5, 3)}");
			Console.WriteLine($"Subtract: {Calculator.Subtract(5, 3)}");
			Console.WriteLine($"Multiply: {Calculator.Multiply(5, 3)}");
			Console.WriteLine($"Divide: {Calculator.Divide(6, 3)}\n");

			// StringProcessor example
			StringProcessor processor = new StringProcessor();

			string lowerCase = processor.ToLowerCase("Hello WORLD!");
			Console.WriteLine($"StringProcessor: ToLowerCase 'Hello WORLD!' = '{lowerCase}'");

			string sanitized = processor.Sanitize("He!llo@");
			Console.WriteLine($"StringProcessor: Sanitize 'He!llo@' = '{sanitized}'");

			bool areEqual = processor.AreEqual("He!llo@", "hello");
			Console.WriteLine($"StringProcessor: AreEqual 'He!llo@' and 'hello' = {areEqual}\n");

			//  uppgift bankaccount
			BankAccount account = new BankAccount();

			// Sätta in pengar
			account.Deposit(1000m);
			Console.WriteLine("Saldo efter insättning: " + account.Balance);

			// Ta ut pengar
			account.Withdraw(200m);
			Console.WriteLine("Saldo efter uttag: " + account.Balance);

			// Försök att ta ut mer pengar än vad som finns på kontot, vilket kastar ett undantag
			try
			{
				account.Withdraw(1000m);
			}
			catch (InvalidOperationException ex)
			{
				Console.WriteLine("Fel: " + ex.Message);
			}
		}
	}
}