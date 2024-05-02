using TopUpApi.Models;

namespace TopUpApi.Service
{
    public interface ICyberSourceHttpClient
    {
        Task<PaymentResponse?> AuthorizeAmountAsync(string amount, string userId);
        Task<bool> CaptureAmountAsync(string transactionId, string amount);
    }
}
