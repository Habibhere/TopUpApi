namespace TopUpApi.Contracts.DTOs
{
    public class AddBeneficiary
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; } = default!;
    }
}
