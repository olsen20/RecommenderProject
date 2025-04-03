using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RecommenderProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AzureRecommenderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AzureRecommenderController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendations([FromBody] UserRequest request)
        {
            var endpointUrl = _configuration["AzureML:Endpoint"];
            var apiKey = _configuration["AzureML:ApiKey"];

            var client = _httpClientFactory.CreateClient();

            var payload = new
            {
                Inputs = new
                {
                    input1 = new[]
                    {
                        new { personId = long.Parse(request.UserId) }  // Azure expects int64
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpointUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await client.SendAsync(httpRequest);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return Content(content, "application/json");
                }

                return StatusCode((int)response.StatusCode, new { error = content });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        public class UserRequest
        {
            public string UserId { get; set; }
        }
    }
}
