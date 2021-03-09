using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TestApp.Filters
{
    /// <summary>
    /// ValidIpAddressFilter action filter responds with Bad Request HttpStatus Code if the IP Address passed in the URL is invalid.
    /// </summary>
    public class ValidIpAddressFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestContext = context;
            var ipAddress = context.ActionArguments.FirstOrDefault(p => p.Key == "ipAddress");
            var ipAddressVal = ipAddress.Value as string;
            
            if (!ValidateIP(ipAddressVal))
            {
                context.Result = new BadRequestObjectResult("Please enter a valid IP address.");
                return Task.FromResult(0);
            }

            return base.OnActionExecutionAsync(context, next);
        }

        private bool ValidateIP(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            return IPAddress.TryParse(ipString, out var ip); 
        }
    }
}
