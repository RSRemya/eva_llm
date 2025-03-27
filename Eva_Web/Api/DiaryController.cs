using Eva_Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Runtime.Intrinsics.X86;
using System;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using LLMIntegration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eva_Rest_API.Controllers
{ 

    [Route("api/[controller]")]
    [ApiController]
    public class DiaryController : ControllerBase
    { 
      
        [Obsolete]
        [HttpPost]
        public string GetDiaryContextForSession([FromForm] string sessionId)
        {

            Guid extractedSessionId;
            if (TryExtractGuid(sessionId, out extractedSessionId))
            {
                return DiaryRepository.LastConversationContextForUser(extractedSessionId);
            }
            return "";
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

        // PUT  , this end point will be used to get all diary entries for the day
        [HttpPut]
        public string GetAllDiaryEntriesForToday()
        {
            return DiaryRepository.AllDiaryEntriesForToday(); 
        }

        // DELETE api/<DiaryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
