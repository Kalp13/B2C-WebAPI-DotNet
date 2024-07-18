using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Azure.Functions.Worker;
using Sample.ExternalIdentities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Eventing.Reader;

namespace csharp.Functions
{
    public static class SignUpValidation
    {
        [Function("SignUpValidation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "SignUpValidation")] HttpRequest req)
        {
            var log = new LoggerFactory().CreateLogger("SignUpValidation");
            // Allowed domains
            string[] allowedDomain = { "live.com", "live.co.za", "gmail.com", "rfktestdomain.onmicrosoft.com" };

            // Check HTTP basic authorization
            if (!Authorize(req, log))
            {
                log.LogWarning("HTTP basic authentication validation failed.");
                return new UnauthorizedResult();
            }

            // Get the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // If input data is null, show block page
            if (data == null)
            {
                return new OkObjectResult(new ResponseContent("ShowBlockPage", "There was a problem with your request."));
            }

            // Print out the request body
            log.LogInformation("Request body: " + requestBody);

            // Get the current user language 
            string language = data.ui_locales == null || data.ui_locales.ToString() == "" ? "default" : data.ui_locales.ToString();
            log.LogInformation($"Current language: {language}");

            // If email claim not found, show block page. Email is required and sent by default.
            if (data.email == null || data.email.ToString() == "" || data.email.ToString().Contains("@") == false)
            {
                return new OkObjectResult(new ResponseContent("ShowBlockPage", "Email name is mandatory."));
            }

            // Get domain of email address
            string domain = data.email.ToString().Split("@")[1];

            // Check the domain in the allowed list
            if (!allowedDomain.Contains(domain.ToLower()))
            {
                return new OkObjectResult(new ResponseContent("ShowBlockPage", $"You must have an account from '{string.Join(", ", allowedDomain)}' to register as an external user for Contoso."));
            }

            // If displayName claim doesn't exist, or it is too short, show validation error message. So, user can fix the input data.
            if (data.displayName == null || data.displayName.ToString().Length < 5)
            {
                return new BadRequestObjectResult(new ResponseContent("ValidationError", "Please provide a Display Name with at least five characters."));
            }

            // Input validation passed successfully, return `Allow` response.
            // TO DO: Configure the claims you want to return
            return new OkObjectResult(new ResponseContent()
            {
                jobTitle = "This value return by the API Connector"//,
                // You can also return custom claims using extension properties.
                //extension_CustomClaim = "my custom claim response"
            });
        }

        private static bool Authorize(HttpRequest req, ILogger log)
        {
            // Get the environment's credentials 
            string username = Environment.GetEnvironmentVariable("BASIC_AUTH_USERNAME", EnvironmentVariableTarget.Process);
            string password = Environment.GetEnvironmentVariable("BASIC_AUTH_PASSWORD", EnvironmentVariableTarget.Process);

            // Returns authorized if the username is empty or not exists.
            if (string.IsNullOrEmpty(username))
            {
                log.LogInformation("HTTP basic authentication is not set.");
                return true;
            }

            // Check if the HTTP Authorization header exist
            if (!req.Headers.ContainsKey("Authorization"))
            {
                log.LogWarning("Missing HTTP basic authentication header.");
                return false;
            }

            // Read the authorization header
            var auth = req.Headers["Authorization"].ToString();

            // Ensure the type of the authorization header id `Basic`
            if (!auth.StartsWith("Basic "))
            {
                log.LogWarning("HTTP basic authentication header must start with 'Basic '.");
                return false;
            }

            // Get the the HTTP basinc authorization credentials
            var cred = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');

            // Evaluate the credentials and return the result
            return cred[0] == username && cred[1] == password;
        }

        [Function("AddClaims")]
        public static async Task<IActionResult> AddClaims([HttpTrigger(AuthorizationLevel.Function, "post", Route = "AddClaims")] HttpRequest req)
        {

            var log = new LoggerFactory().CreateLogger("AddClaims");

            // Check HTTP basic authorization
            if (!Authorize(req, log))
            {
                log.LogWarning("HTTP basic authentication validation failed.");
                return new UnauthorizedResult();
            }

            // Get the request body
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // If input data is null, show block page
            if (data == null)
            {
                return new OkObjectResult(new ResponseContent("ShowBlockPage", "There was a problem with your request."))
                {
                    ContentTypes = { "application/json" },
                    StatusCode = StatusCodes.Status200OK,
                };
                //return new OkObjectResult(new ResponseContent("ShowBlockPage", "There was a problem with your request."));
            }

            // Print out the request body
            log.LogInformation("Request body: " + requestBody);

            // Get the current user language 
            string language = data.ui_locales == null || data.ui_locales.ToString() == "" ? "default" : data.ui_locales.ToString();
            log.LogInformation($"Current language: {language}");

            // If email claim not found, show block page. Email is required and sent by default.
            if (!IsEmailValid(data))
            {
                return new OkObjectResult(new ResponseContent("ShowBlockPage", "Email name is mandatory."));
            }

            // Get domain of email address
            string domain = GetDomain(data);

            // If displayName claim doesn't exist, or it is too short, show validation error message. So, user can fix the input data.
            //if (data.displayName == null || data.displayName.ToString().Length < 5)
            //{
            //    return new BadRequestObjectResult(new ResponseContent("ValidationError", "Please provide a Display Name with at least five characters."));
            //}

            // Input validation passed successfully, return `Allow` response.
            // TO DO: Configure the claims you want to return
            return new OkObjectResult(new ResponseContent()
            {
                jobTitle = "This is a fake job title"//,
                // You can also return
            })
            { 
                ContentTypes = { "application/json" },
                StatusCode = StatusCodes.Status200OK,
                Value = new { jobTitle = "This is a fake job title" }
            };
        }

        private static string GetDomain(dynamic data)
        {
            string email = String.Empty;
            if (!String.IsNullOrWhiteSpace(data.email) && data.email.Contains('@'))
            {
                email = data.email;
            }
            else if (data.emails.Count > 0 && ((string[])data.emails).All(x => !String.IsNullOrWhiteSpace(x) && x.Contains('@')))
            {
                email =  ((string[])data.emails).FirstOrDefault(x => !String.IsNullOrWhiteSpace(x) && x.Contains('@'));
            }

            return email.ToString().Split("@")[1];
        }

        private static bool IsEmailValid(dynamic data)
        {
            if (!String.IsNullOrWhiteSpace(data.email) && data.email.Contains('@'))
            {
                return true;
            }
            else if (data.emails.Count > 0 && ((string[])data.emails).All(x => !String.IsNullOrWhiteSpace(x) && x.Contains('@')))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
