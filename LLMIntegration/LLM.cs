namespace LLMIntegration
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Eva_Repository;
    using UglyToad.PdfPig;
    public class LLM
    {
        // The base URL for your Hugging Face model API, todo: move below to a constuctor
        // model can be found at https://huggingface.co/Orenguteng/Llama-3.1-8B-Lexi-Uncensored-V2-GGUF
        private static readonly string apiUrl = "https://hzi9y6a1bxydu0pv.us-east-1.aws.endpoints.huggingface.cloud/v1/chat/completions";//Q4 version
            //"https://co2fna3hec3e4284.us-east-1.aws.endpoints.huggingface.cloud/v1/chat/completions"; // Q8 version
            //hermes/"https://upqzwlp4h3eczudt.us-east-1.aws.endpoints.huggingface.cloud/v1/chat/completions";  
        private static readonly string bearerToken = "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"; // Replace with your Hugging Face Bearer token
        const int max_tokens = 8000;
        const float temp = 0.8f;
        private string kbContext;
        public LLM(string apiUrl, string bearerToken, int max_tokens, float temp, string kbContext, List<string> existingConversationContext = null)
        {
            apiUrl = apiUrl; bearerToken = bearerToken; max_tokens = max_tokens; temp = temp; kbContext = kbContext; conversationContext = existingConversationContext;
        }
        public LLM(){}

        public LLM(List<string> existingConversationContext) { conversationContext = existingConversationContext; }

        public List<string> ConversationContexts { get { return conversationContext; } }
        // Store conversation context (user and assistant messages),  this needs to be persisted for a user in DB so that the context never dies
        //similar to diary service, the conversation context needs to be persisted for the user always so that the API mode can work
        private List<string> conversationContext = new List<string>();
       
        public static async Task<string> DiarySummary(List<string> conversationHistory)
        {  
            if (conversationHistory == null || conversationHistory.Count == 0) 
                return "";  

            conversationHistory.RemoveAll(conv=> conv.Contains("system"));
            string systemPrompt = CharacterRepository.LoadCharacter(1).DiaryPrompt;           
            var allmessages = "";
            GetMessagesFromContext(conversationHistory).ToList().ForEach(a => allmessages += a); 
            conversationHistory.Insert(0,$"{{\"role\": \"system\", \"content\": \"{systemPrompt} {allmessages.Replace('{',' ').Replace('}',' ')}\"}}");
            conversationHistory.RemoveRange(1, conversationHistory.Count - 1);
            var requestBody = new
            {
                model = "gpt-3.5-turbo", // Specify the OpenAI model (e.g., "gpt-3.5-turbo", "gpt-4", etc.)
                messages = GetMessagesFromContext(conversationHistory),
                max_tokens = max_tokens,
                temperature = temp, // Adjust temperature for randomness in responses
                stream = false // Set to true if you want streaming responses
            };

            // Make the API call to OpenAI-compatible backend
            string responseContent = await GetOpenAIResponse(requestBody);

            // Maintain conversation context (the assistant's response)
            string diaryEntry = ExtractAssistantMessage(responseContent);           
            return diaryEntry;  
        }
        public   async Task<string> Talk(string userInput)
        {
            //testing only
            // var summary= await DiarySummary(new List<string>(conversationContext));
            var character = CharacterRepository.LoadCharacter(1);
            string kbContext = character.CombinedKB;
            string systemPrompt = character.SystemPrompt;

            if (conversationContext.Count == 0)
            {
                conversationContext.Add($"{{\"role\": \"system\", \"content\": \"{systemPrompt}, you can also answer questions using information extracted from the relevant information provided next {kbContext} \"}}");

            }
            else { 
                conversationContext.RemoveAt(0);
                conversationContext.Insert(0, $"{{\"role\": \"system\", \"content\": \"{systemPrompt}, you can also answer questions using information extracted from the relevant information provided next {kbContext} \"}}"); 
            }
            // Add the user's message to the conversation context
            conversationContext.Add($"{{\"role\": \"user\", \"content\": \"{userInput}\"}}");
            

                // Create the JSON request body
                var requestBody = new
                {
                    model = "gpt-3.5-turbo", // Specify the OpenAI model (e.g., "gpt-3.5-turbo", "gpt-4", etc.)
                    messages = GetMessagesFromContext(),
                    max_tokens = max_tokens,
                    temperature = temp, // Adjust temperature for randomness in responses
                    stream = false // Set to true if you want streaming responses
                };

                // Make the API call to OpenAI-compatible backend
                string responseContent = await GetOpenAIResponse(requestBody);

                // Maintain conversation context (the assistant's response)
                string assistantMessage = ExtractAssistantMessage(responseContent).ToLower();
                if (!string.IsNullOrEmpty(assistantMessage))
                {
                    conversationContext.Add($"{{\"role\": \"assistant\", \"content\": \"{assistantMessage}\"}}");
                }
                return assistantMessage;
                
               
            
        }
        private static object[] GetMessagesFromContext(List<string> conversationContext)
        {
            string[] contextLines = conversationContext.ToArray();// conversationContext.ToString().Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            var messages = new object[contextLines.Length];

            for (int i = 0; i < contextLines.Length; i++)
            {
                var role = contextLines[i].Contains("\"role\": \"system\"")?"system":(contextLines[i].Contains("\"role\": \"user\"") ? "user" : "assistant");
                var content = contextLines[i].Substring(contextLines[i].IndexOf("\"content\":") + 10).Trim(' ', '\"', '}');

                messages[i] = new
                {
                    role = role,
                    content = content
                };
            }

            return messages;
        }

        // Method to build the messages array from the conversation context (this ensures the conversation history is included)
        private  object[] GetMessagesFromContext()
        {
            string[] contextLines = conversationContext.ToArray();// conversationContext.ToString().Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            var messages = new object[contextLines.Length];

            for (int i = 0; i < contextLines.Length; i++)
            {
                var role = contextLines[i].Contains("\"role\": \"user\"") ? "user" : "assistant";
                var content = contextLines[i].Substring(contextLines[i].IndexOf("\"content\":") + 10).Trim(' ', '\"', '}');

                messages[i] = new
                {
                    role = role,
                    content = content
                };
            }

            return messages;
        }

        // Method to make a request to OpenAI API
        private static async Task<string> GetOpenAIResponse(object requestBody)
        {
            using (var client = new HttpClient())
            {
                // Add the Authorization header with the Bearer token
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

                // Serialize the request body to JSON
                string jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                // Send the POST request
                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Read and return the response body as a string
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
        }

        // Method to extract the assistant's message from the OpenAI response
        private static string ExtractAssistantMessage(string responseContent)
        {
            if (string.IsNullOrEmpty(responseContent))
            {
                Console.WriteLine("Error in receiving a valid response.");
                return null;
            }

            try
            {
                // Deserialize the response into an object
                dynamic response = JsonConvert.DeserializeObject(responseContent);

                // Extract the assistant's message from the response
                string assistantMessage = response?.choices?[0]?.message?.content;

                return assistantMessage;
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Console.WriteLine($"Error processing the response: {ex.Message}");
                return null;
            }
        }


    }
}
