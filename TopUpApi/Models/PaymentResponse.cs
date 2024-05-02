using Newtonsoft.Json;

namespace TopUpApi.Models
{
    public class ErrorInformation
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class PaymentResponse
    {
        [JsonProperty("submitTimeUtc")]
        public DateTime SubmitTimeUtc { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("errorInformation")]
        public ErrorInformation ErrorInformation { get; set; }
        public string Id { get; set; }
        public bool IsDefaultAuthorised { get; set; } = false;
        public bool IsAuthorizedStatus => !string.IsNullOrEmpty(Status) && Status == "AUTHORIZED";
    }
}
