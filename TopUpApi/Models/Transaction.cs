namespace TopUpApi.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }

    public class TestTransactionData
    {
        public static List<Transaction> GetTransactions()
        {
            var transactions = new List<Transaction>
            {
                // Sample transactions for different users and beneficiaries
                new Transaction { Id = 1, UserId = "user1", BeneficiaryId = 1, Amount = 50, Date = new DateTime(2024, 5, 1) },
                new Transaction { Id = 2, UserId = "user1", BeneficiaryId = 1, Amount = 30, Date = new DateTime(2024, 5, 3) },
                new Transaction { Id = 3, UserId = "user2", BeneficiaryId = 3, Amount = 20, Date = new DateTime(2024, 5, 4) },
                new Transaction { Id = 4, UserId = "user3", BeneficiaryId = 5, Amount = 100, Date = new DateTime(2024, 5, 7) },
                new Transaction { Id = 5, UserId = "user4", BeneficiaryId = 6, Amount = 200, Date = new DateTime(2024, 5, 20) }
            };

            return transactions;
        }
    }

    public class TestBeneficiaryData
    {
        public static List<Beneficiary> GetBeneficiaries()
        {
            var beneficiaries = new List<Beneficiary>
            {
                new() { Id = 1, UserId = "user1", Nickname = "Beneficiary 1" },
                new() { Id = 2, UserId = "user1", Nickname = "Beneficiary 2" },
                new() { Id = 3, UserId = "user2", Nickname = "Beneficiary 1" },
                new() { Id = 4, UserId = "user2", Nickname = "Beneficiary 2" },
                new() { Id = 5, UserId = "user2", Nickname = "Beneficiary 3" },
                new() { Id = 6, UserId = "user2", Nickname = "Beneficiary 4" },
                new() { Id = 7, UserId = "user2", Nickname = "Beneficiary 5" },
                new() { Id = 8, UserId = "user2", Nickname = "Beneficiary 6" },
                new() { Id = 9, UserId = "user3", Nickname = "Beneficiary 1" }
            };

            return beneficiaries;
        }
    }
}
