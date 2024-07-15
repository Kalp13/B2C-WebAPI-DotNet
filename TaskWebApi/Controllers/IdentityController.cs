using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpPost("LookupIdentity")]
        public async Task<IActionResult> LookupIdentity()
        {
            this.Request.HttpContext.Items.Add("TestApi", "Rudolph Test");
            return Ok();
        }

        //[HttpPost("OnboardUser")]
        //public Task<IActionResult> OnboardUser(string partyId, string? name, string? surname, string? contactNumber, string? countryCode)
        //{

        //    return _customerManagementService.Onboard(partyId, name, surname, contactNumber, countryCode);
        //}
    }
}
