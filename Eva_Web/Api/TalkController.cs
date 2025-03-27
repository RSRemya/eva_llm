using Eva_Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using LLMIntegration;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eva_Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalkController : ControllerBase
    {
        // GET: api/<TalkController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TalkController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TalkController>, input to BYOM node, returns output from model
        [HttpPost]
        public async Task<string> Post([FromForm] string sessionId, [FromForm] string userInput)
        {
            Guid extracteduserId;
            try
            {
                if (TryExtractGuid(sessionId, out extracteduserId))
                {
                    var existingConversations = ConversationRepository.LoadUserConversations(extracteduserId) ?? new List<ConversationContext>();
                    bool newConversation = existingConversations.Count == 0;

                    LLM llm = newConversation ? new LLM() : new LLM(existingConversations.Select(conv => conv.Conversation).ToList());

                    var modelResponseForQuery = await llm.Talk(userInput);
                    var lastresponseInOpenAIFormat = llm.ConversationContexts[llm.ConversationContexts.Count - 1];

                    var saved = newConversation ? ConversationRepository.SaveUserConversationContexts(extracteduserId, llm.ConversationContexts.Select(conv => new ConversationContext { Conversation = conv, UserId = extracteduserId, Date=DateTime.UtcNow }).ToArray()) : ConversationRepository.SaveUserConversationContexts(extracteduserId, llm.ConversationContexts.GetRange(llm.ConversationContexts.Count - 2, 2).Select(conv => new ConversationContext { Conversation = conv, UserId = extracteduserId , Date = DateTime.UtcNow }).ToArray()) ;

                    return saved ? modelResponseForQuery : "Sorry i am not available at the moment";
                }
            }
            catch (Exception ex) { }
            return "Sorry i am not available at the moment";
        }

        static bool TryExtractGuid(string input, out Guid guid)
        {
            // Regular expression pattern for matching GUIDs
            string pattern = @"\b[a-fA-F0-9]{8}\b-[a-fA-F0-9]{4}\b-[a-fA-F0-9]{4}\b-[a-fA-F0-9]{4}\b-[a-fA-F0-9]{12}\b";
            Regex regex = new Regex(pattern);

            Match match = regex.Match(input);
            if (match.Success && Guid.TryParse(match.Value, out guid))
            {
                return true;
            }

            guid = Guid.Empty;
            return false;
        }
 

        // PUT api/<TalkController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TalkController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
