using HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.IO.Pipes;
using System.Net.Http.Headers;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person() {Name ="CharlesWai", Age = 27 };
            Console.WriteLine($"Hello {person.Name}, you are {person.Age} years old");
            Console.WriteLine("Hello World!");
            Console.WriteLine();
            Console.WriteLine("Input your question to call ChatGPT API:");
            Console.WriteLine("continue chat(input somthing what you want to know) or quit(using 'quit' to quit conversation)...");
            string question = Console.ReadLine();
            while (question != null)
            {
                if (question == "quit") { break; }
                if (question == "") { break; }
                string responseContent = CallChatGPTAPI(question).Result;
                ChatCompletionResponse response = JsonConvert.DeserializeObject<ChatCompletionResponse>(responseContent);
                string message = response.choices[0].message.content;
                Console.WriteLine();
                Console.WriteLine(message);
                Console.WriteLine();
                question = Console.ReadLine();
            }
        }

        #region ChatGPT

        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task<string> CallChatGPTAPI(string message)
        {
            string apiKey = "";
            string keyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.txt");
            if (!File.Exists(keyPath))
            {
                return $"Call ChatGPTAPI Error! Not found the {"Key.txt"} file on the path of {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}";
            }
            using (FileStream stream = File.OpenRead(keyPath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    line = reader.ReadLine();
                    
                    if (line != null)
                    {
                        apiKey = line;
                    }
                    else
                    {
                        return $"Call ChatGPTAPI Error! the {"Key.txt"} file content on the path of {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)} is empty";
                    }
                }
            }
            
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "system", content = "You are a helpful assistant." }, new { role = "user", content = message } }
            };
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        #endregion ChatGPT
    }

    

    public class ChatCompletionResponse
    {
        public string id { get; set; }
        public string @object { get; set; }
        public long created { get; set; }
        public string model { get; set; }
        public Usage usage { get; set; }
        public List<Choice> choices { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

}
