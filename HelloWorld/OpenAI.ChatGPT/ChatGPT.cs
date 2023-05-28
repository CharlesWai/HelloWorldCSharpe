using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace OpenAI.ChatGPT
{
    public class ChatGPT
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> CallChatGPTAPI(string message)
        {
            string apiKey = "YOUR_API_KEY";
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var requestData = new
            {
                messages = new[] { new { role = "system", content = "You are a helpful assistant." }, new { role = "user", content = message } }
            };
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}


