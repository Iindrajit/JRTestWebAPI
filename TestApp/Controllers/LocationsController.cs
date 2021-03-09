using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using TestApp.Filters;

namespace TestApp.Controllers
{
    [Route("api/location")]
    [ApiController]
    [ServiceFilter(typeof(ValidIpAddress))]
    public class LocationsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public LocationsController(IConfiguration config, ILogger<LocationsController> logger) 
        {
            _config = config;
            _logger = logger;
        }

        [HttpGet("{ipAddress}")]
        public async Task<IActionResult> GetCityByIp(string ipAddress)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var uri = _config.GetValue<string>("IpToGeoLocationApi").Replace("ipAdd", ipAddress);
                _logger.LogInformation($"GetCityByIp: {uri}");

                using (HttpResponseMessage httpResponse = await httpClient.GetAsync(uri))
                {
                    if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = await httpResponse.Content.ReadAsStringAsync();
                        JObject jObject = JObject.Parse(apiResponse);
                        if(jObject != null)
                        {
                            if (jObject.SelectToken("error") != null && jObject.SelectToken("error").HasValues)
                               throw new Exception(jObject.SelectToken("error.info").ToString());
                            
                            var city = jObject.SelectToken("city").ToString();
                            return Ok(city);
                        }
                    }

                    return NotFound();
                }
            }
        }
    }
}