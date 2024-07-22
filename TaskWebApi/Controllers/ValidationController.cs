using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidationController(IAzureAdLookupService azureAdLookupService,
        IEnumerable<IFederationResolver> federationResolvers) : ControllerBase
    {
        private readonly IAzureAdLookupService _azureAdLookupService = azureAdLookupService;
        private readonly IEnumerable<IFederationResolver> _federationResolvers = federationResolvers;

        [HttpGet("ValidateActive")]
        public IActionResult ValidateActive()
        {
            return Ok("API Active");
        }

        [HttpGet("ValidateAccessCode")]
        public async Task<IActionResult> ValidateAccessCode([FromQuery] string accessCode)
        {
            // Validate the access code
            if (accessCode == "54321")
            {
                return Ok("Success");
            }
            else
            {
                return Conflict();
            }
        }

        [HttpGet("LookupIdentity")]
        public async Task<ActionResult<IdentityLookupResponse>> LookupIdentity(string email)
        {
            try
            {
                AzureADB2CUserQueryResponse usersMatchingEmail = await _azureAdLookupService.Lookup(email);
                var identityResponse = new IdentityLookupResponse();
                if (usersMatchingEmail?.value?.Length > 0 && usersMatchingEmail.value?.FirstOrDefault()?.objectId != null)
                {
                    identityResponse.IdentityProviders = new List<string>
                {
                    "micrososft.online.com"
                };
                    identityResponse.Exists = true;
                    identityResponse.IsOnboarded = true;
                    return identityResponse;
                }
                string domain = email[(email.IndexOf('@') + 1)..];
                IEnumerable<Task<AuthenticationIdentity?>> federationProviderTasks = _federationResolvers.Select(resolver => resolver.Resolve(domain));
                IEnumerable<AuthenticationIdentity?> activeProviders = (await Task.WhenAll(federationProviderTasks)).Where(response => !string.IsNullOrWhiteSpace(response?.identityProvider));
                if (activeProviders.Any())
                {
                    return Ok(new IdentityLookupResponse
                    {
                        Exists = false,
                        IdentityProviders = activeProviders.Select(s => s?.identityProvider!).ToList(),
                        Force = (activeProviders.Any(x => x.force))
                    });
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("OnboardUser")]
        public Task<FullContantResponse> OnboardUser(string partyId, string? name, string? surname, string? contactNumber, string? countryCode)
        {
            TaskCompletionSource<FullContantResponse> tcs = new TaskCompletionSource<FullContantResponse>();
            tcs.TrySetResult(new FullContantResponse
            {
                PartyId = partyId,
                Name = name,
                Surname = surname,
                ContactNumber = contactNumber,
                CountryCode = countryCode
            });
            return tcs.Task;
        }
    }
}
