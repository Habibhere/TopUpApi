namespace TopUpApi.Models
{
    public class AuthorizationResponse
    {
        public bool IsAuthorised { get; set; } = false;
        public string PublicMessage { get; set; } = string.Empty;
        public string Id { get; set; }
    }
}
