using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public ListingsController(IConfiguration config, ILogger<ListingsController> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// GetQuote gets quote. It filters out listings that don't support the number of passengers. With the remaining listings,
        /// it calculates total price and returns the results sorted by total price.
        /// </summary>
        /// <param name="numberOfPassengers"></param>
        /// <returns></returns>
        [HttpGet(), Route("{numberOfPassengers:int:min(1)}")]
        public async Task<IActionResult> GetQuote(int numberOfPassengers)
        {
            _logger.LogInformation($"GetQuote: Number of passengers: {numberOfPassengers}");

            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpResponseMessage httpResponse = await httpClient.GetAsync(_config.GetValue<string>("QuotesApi")))
                {
                    if (httpResponse != null && httpResponse.IsSuccessStatusCode)
                    {
                        var apiResponse = await httpResponse.Content.ReadAsStringAsync();
                        _logger.LogInformation($"Quote: {apiResponse}");

                        var listingInfo = JsonConvert.DeserializeObject<ListingInfo>(apiResponse);

                        listingInfo.Listings = listingInfo.Listings.Where(x => x.VehicleType.MaxPassengers >= numberOfPassengers).ToList();

                        if (listingInfo.Listings.Count == 0)
                            return NoContent();

                        listingInfo.Listings.ForEach(l => l.TotalPrice = Math.Round(l.PricePerPassenger * l.VehicleType.MaxPassengers, 2));
                        listingInfo.Listings = listingInfo.Listings.OrderBy(l => l.TotalPrice).ToList();

                        return Ok(listingInfo);
                    }

                    return NotFound();
                }
            }
        }
    }
}