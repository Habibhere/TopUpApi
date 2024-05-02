using System.Globalization;
using TopUpApi.Models;

namespace TopUpApi.Service
{
    public class TopUpService : ITopUpService
    {
        private readonly ICyberSourceHttpClient _cyberSourceHttpClient;
        private readonly int AllowedPerMonthAmount = 3000;
        public TopUpService(ICyberSourceHttpClient cyberSourceHttpClient)
        {
            _cyberSourceHttpClient = cyberSourceHttpClient;
        }
        public IList<Beneficiary> GetBeneficiaries(string userId)
        {
            var beneficiaries = TestBeneficiaryData.GetBeneficiaries();

            return beneficiaries.Where(b => b.UserId == userId).ToList();
        }

        public async Task<IList<TopUpOption>> GetTopUpOptions()
        {
            var options = new List<TopUpOption>
            {
                new() { Id = 1, Amount = 5 },
                new() { Id = 2, Amount = 10 },
                new() { Id = 3, Amount = 20 },
                new() { Id = 3, Amount = 30 },
                new() { Id = 3, Amount = 50 },
                new() { Id = 3, Amount = 75 },
                new() { Id = 3, Amount = 100}
            };

            return options;
        }

        public async Task<AuthorizationResponse> CanTopUp(string userId, decimal amount, bool isUserVerified, int beneficiaryId)
        {
            AuthorizationResponse paymentResponse = new AuthorizationResponse();
            decimal monthlyLimit = isUserVerified ? 500 : 1000; // Monthly limit based on verification status
            if (amount > monthlyLimit)
            {
                return paymentResponse;

            }
            var totalTopUpAmount = GetTotalTopUpAmountForCurrentMonth(userId);
            if (totalTopUpAmount > AllowedPerMonthAmount)
            {
                return paymentResponse;

            }

            var beneficiaryAmount = GetBeneficiaryAmountForCurrentMonth(userId, beneficiaryId);
            if (beneficiaryAmount > monthlyLimit )
            {
                return paymentResponse;
            }

            var amountWithCharge = amount + 1;
            var response = await UserFundsValidation(userId, amountWithCharge);
            if (response is not null)
            {
                if (response.IsAuthorizedStatus)
                {
                    paymentResponse.IsAuthorised = true;
                    paymentResponse.Id = response.Id;
                }
                paymentResponse.PublicMessage = response.ErrorInformation.Message;
                return paymentResponse;
            }
            return paymentResponse;
        }

        private async Task<PaymentResponse?> UserFundsValidation(string userId, decimal amount)
        {
            return await _cyberSourceHttpClient.AuthorizeAmountAsync(amount.ToString(CultureInfo.InvariantCulture), userId);
        }

        public async Task<bool> TopUp(TopUpRequest request)
        {
            var amountWithCharge = request.Amount + 1;
            return await _cyberSourceHttpClient.CaptureAmountAsync(request.TransactionId, amountWithCharge.ToString(CultureInfo.InvariantCulture));

            //Here in the end a DB call can be made to record the transaction and its metadata
        }
        public async Task<bool> AddBeneficiaryAsync(AddBeneficiaryRequest request)
        {
            var userBeneficiaries = GetBeneficiaries(request.UserId);
            if (userBeneficiaries.Count > 5)
            {
                return false;
            }
            return true;
        }

        private decimal GetTotalTopUpAmountForCurrentMonth(string userId)
        {
            var transactions = TestTransactionData.GetTransactions();

            // Get the current month and year
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // Calculate the total top-up amount for the current month
            return transactions
                 .Where(t => t.Date.Month == currentMonth && t.Date.Year == currentYear && t.UserId == userId)
                 .Sum(t => t.Amount);
        }

        private decimal GetBeneficiaryAmountForCurrentMonth(string userId, int beneficiaryId)
        {
            var transactions = TestTransactionData.GetTransactions();

            // Get the current month and year
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return transactions
                .Where(t => t.Date.Month == currentMonth && t.Date.Year == currentYear
                                                         && t.UserId == userId && t.BeneficiaryId == beneficiaryId)
                .Sum(t => t.Amount);
        }
    }
}
