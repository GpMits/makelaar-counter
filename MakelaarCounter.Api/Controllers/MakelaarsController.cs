using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MakelaarCounter.Api.Services;

namespace MakelaarCounter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MakelaarsController : ControllerBase
    {
        private readonly IOfferService _offerService;
        
        public MakelaarsController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet("offer-count/{searchQuery}/{top:int}")]
        public async Task<IActionResult> GetOfferCount(string searchQuery, int top)
        {
            var makelaarCount = await _offerService.GetMakelaarOfferCount(searchQuery, top);
            return !makelaarCount.Any() 
                ? Problem("Something went wrong, please try again later.") 
                : Ok(makelaarCount);
        }
        
        [HttpGet("offer-count-amsterdam")]
        public async Task<IActionResult> GetOfferCountAmsterdam()
        {
            return await GetOfferCount("/amsterdam", 10);
        }
        
        [HttpGet("offer-count-amsterdam-tuin")]
        public async Task<IActionResult> GetOfferCountAmsterdamTuin()
        {
            return await GetOfferCount("/amsterdam/tuin", 10);
        }
    }
}
