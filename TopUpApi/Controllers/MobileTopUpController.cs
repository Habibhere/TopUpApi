using Microsoft.AspNetCore.Mvc;
using TopUpApi.Models;
using TopUpApi.Service;

namespace TopUpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MobileTopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;

        public MobileTopUpController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }
        [HttpPost("AddBeneficiaryRequest")]
        public async Task<IActionResult> AddBeneficiary([FromBody] AddBeneficiaryRequest request)
        {
            var beneficiaryAdded = await _topUpService.AddBeneficiaryAsync(request);
            return Ok(beneficiaryAdded);
        }


        [HttpGet("beneficiaries")]
        public IActionResult GetBeneficiaries(string userId)
        {
            var beneficiaries = _topUpService.GetBeneficiaries(userId);
            return Ok(beneficiaries);
        }

        [HttpGet("topupoptions")]
        public IActionResult GetTopUpOptions()
        {
            var options = _topUpService.GetTopUpOptions();
            return Ok(options);
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] TopUpRequest request)
        {
            // Validate request
            var response = await _topUpService.CanTopUp(request.UserId, request.Amount, request.IsUserVerified, request.BeneficiaryId);
            if (!response.IsAuthorised)
            {
                return BadRequest(response.PublicMessage);
            }

            // Perform top-up
            request.TransactionId = response.Id;
            var topStatus = await _topUpService.TopUp(request);
            if (topStatus)
            {
                return Ok("Top-up successful.");
            }
            return BadRequest("Top-up failed");
        }
    }
}
