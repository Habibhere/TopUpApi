using System.ComponentModel.DataAnnotations;

namespace TopUpApi.Models
{
    public class AddBeneficiaryRequest
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "{0} Can Only Have Between {2} and {1} Characters.")]
        public string NickName { get; set; } = default!;
        public string UserId { get; set; }
        public int Id { get; set; }
    }
}
