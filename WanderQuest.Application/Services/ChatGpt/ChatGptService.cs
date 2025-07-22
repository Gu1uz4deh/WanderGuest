using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WanderQuest.Application.Services.ChatGpt
{
    public class ChatGptService : IChatGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "K`ENDI_API_KEYINIZI_BURAYA_YAPISTIRIN";

        public ChatGptService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> AskChatGptAsync(string userMessage)
        {
            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Sen yardımcı bir yazılım asistanısın." },
                    new { role = "user", content = userMessage }
                }
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseContent);
            return result.choices[0].message.content.ToString();
        }
    }
}