using TopUpApi.Contracts.DTOs;

namespace TopUpApi.Contracts.Abstract
{
    public interface IAddBeneficiaryService
    {
        Task<bool> AddBeneficiaryAsync(AddBeneficiary request);
    }
}
