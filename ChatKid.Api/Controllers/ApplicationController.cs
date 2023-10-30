/*using ChatKid.Application.Models.RequestModels.Advertising;
using ChatKid.Application.IServices;
using ChatKid.Application.Models.ViewModels.AdvertisingViewModels;
using ChatKid.Application.Models.ViewModels.OtpViewModels;
using ChatKid.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace ChatKid.Api.Controllers
{
    [Route("application")]
    [ApiController]
    [AllowAnonymous]
    public class ApplicationController : ControllerBase
    {
        [HttpGet("encrypt-aes/{value}")]
        public async Task<IActionResult> EncryptAes([FromRoute] string value)
        {
            return Ok(value.EncryptAes());
        }
        [HttpGet("decrypt-aes/{token}")]
        public async Task<IActionResult> DecryptAes([FromRoute] string token)
        {
            return Ok(token.DecryptAes());
        }


        [HttpGet("parse-query-string")]
        public IActionResult ParseQueryString([FromQuery] IEnumerable<Dictionary<string, string>> filters)
        {
           
            return Ok(filters);
        }

        private Dictionary<string, object> ParseQueryParameters(string queryString)
        {
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(queryString);
            var parsedQuery = new Dictionary<string, object>();

            foreach (string key in queryParameters.AllKeys)
            {
                string[] parts = key.Split(new[] { "[", "]", "$" }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, object> currentLevel = parsedQuery;

                for (int i = 0; i < parts.Length - 1; i++)
                {
                    string part = parts[i];

                    if (!currentLevel.ContainsKey(part))
                    {
                        currentLevel[part] = new Dictionary<string, object>();
                    }

                    currentLevel = (Dictionary<string, object>)currentLevel[part];
                }

                string finalKey = parts[parts.Length - 1];
                currentLevel[finalKey] = queryParameters[key];
            }

            return parsedQuery;
        }
    }
}
*/