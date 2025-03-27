using Eva_Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eva_Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        // GET: api/<ConversationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ConversationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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

        public async Task<IActionResult> Post([FromForm] string sessionId, [FromForm] string diaryEntry)
        {
            Guid extractedSessionId;

            _ = Task.Run(() =>
            {
                try
                {
                    if (TryExtractGuid(sessionId, out extractedSessionId))
                    {
                        DiaryRepository.SaveOrUpdateDiary(new DiaryEntry
                        {
                            UserId = extractedSessionId,
                            DiaryEntryDate = DateTime.UtcNow,
                            Entry = diaryEntry
                        });
                    }
                }
                catch { }
            });


            // Return immediately
            return Accepted();
        } 


        // PUT api/<ConversationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ConversationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
