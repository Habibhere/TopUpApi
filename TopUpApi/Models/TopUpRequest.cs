namespace TopUpApi.Models
{
    public class TopUpRequest
    {
        public string UserId { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public bool IsUserVerified { get; set; } = false;
    }
}
