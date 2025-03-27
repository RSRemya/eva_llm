using Newtonsoft.Json;

namespace Eva_Web.Models
{
    public class MessageModel
    {
        [JsonProperty("role")]
        public string Sender { get; set; }

        [JsonProperty("content")]
        public string Text { get; set; }
    }
}