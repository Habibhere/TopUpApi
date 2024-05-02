using System.Text.Json.Serialization;

namespace TopUpApi.Models
{
    public class Beneficiary
    {
        public int Id { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        public string Nickname { get; set; }
    }
}
