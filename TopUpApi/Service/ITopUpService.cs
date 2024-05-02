using TopUpApi.Models;

namespace TopUpApi.Service
{
    public interface ITopUpService
    {
        IList<Beneficiary> GetBeneficiaries(string userId);
        Task<IList<TopUpOption>> GetTopUpOptions();
        Task<AuthorizationResponse> CanTopUp(string userId, decimal amount, bool isUserVerified, int beneficiaryId);
        Task<bool> TopUp(TopUpRequest request);
        Task<bool> AddBeneficiaryAsync(AddBeneficiaryRequest request);
    }
}
