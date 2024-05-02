using TopUpApi.Contracts.Abstract;
using TopUpApi.Contracts.DTOs;

namespace TopUpApi.Concrete.Application
{
    public class AddBeneficiaryService : IAddBeneficiaryService
    {
        public async Task<bool> AddBeneficiaryAsync(AddBeneficiary request)
        {
            return true;
        }
    }
}
