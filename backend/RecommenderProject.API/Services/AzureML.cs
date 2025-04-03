using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace RecommenderProject.API.Services;


public class AzureML
{
    private const string endpointUrl = "https://<your-endpoint>.azurewebsites.net/score";
    private const string apiKey = "<your-api-key>";

    public async Task<List<string>> GetAzureRecommendationsAsync(string itemId)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            Inputs = new
            {
                input1 = new[]
                {
                    new { itemId = itemId }
                }
            },
            GlobalParameters = new { }
        };

        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpointUrl, content);
        var responseJson = await response.Content.ReadAsStringAsync();

        dynamic parsed = JsonConvert.DeserializeObject(responseJson)!;
        return parsed?.Results?.output1?[0]?.recommendations?.ToObject<List<string>>() ?? new List<string>();
    }
}
