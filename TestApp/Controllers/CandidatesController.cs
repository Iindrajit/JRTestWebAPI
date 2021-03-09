using Microsoft.AspNetCore.Mvc;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/candidate")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCandidate()
        {
            var candidate = new Candidate { Name = "test", Phone = "test" };
            return Ok(candidate);
        }
    }
}