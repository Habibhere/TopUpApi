namespace TopUpApi.Models
{
    public class PaymentInstrumentInfo
    {
        public string CardNumber { get; set; } = string.Empty;
        public string ExpirationMonth { get; set; } = string.Empty;
        public string ExpirationYear { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
    }
}
