using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Console_OpenAi
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Faça uma pergunta:");
			Console.WriteLine();

			var pergunta = Console.ReadLine();

			//chamada openAI
			var resposta = ChamadaOpenAI(250, pergunta, "text-davinci-002", 0.7, 1, 0, 0);
			Console.WriteLine(resposta);

		}

		private static string ChamadaOpenAI(int tokens, string input, string engine,
			double temperature, int topP, int frequencyPenalty, int presencePenalty)
		{
			var opeAIKey = "";

			var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";

			try 
			{
				using (var httpclient = new HttpClient())
				{
					using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
					{
						request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + opeAIKey);
						request.Content = new StringContent("{\n  \"prompt\": \"" + input + "\",\n  \"temperature\": " +
								temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n  \"top_p\": " + topP +
								",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = httpclient.SendAsync(request).Result;
						var json = response.Content.ReadAsStringAsync().Result;

						dynamic dynObj = JsonConvert.DeserializeObject(json);

						if (dynObj != null)
							return dynObj.choices[0].text.ToString();
					}
				}
			}
			catch (Exception ex) 
			{ 
				Console.WriteLine(ex.Message);
			}
			return null;
		}

	}
}
