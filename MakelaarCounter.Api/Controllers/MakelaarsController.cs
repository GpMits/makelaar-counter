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

        /// <summary>
        /// Gets the <paramref name="top"/> makelaars with the most house sale offers base on the search query
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        [HttpGet("offer-count/{searchQuery}/{top:int}")]
        public async Task<IActionResult> GetMakelaarOfferCount(string searchQuery, int top)
        {
            var makelaarCount = await _offerService.GetMakelaarOfferCount(searchQuery, top);
            return !makelaarCount.Any() 
                ? Problem("Something went wrong, please try again later.") 
                : Ok(makelaarCount);
        }
        
        /// <summary>
        /// Gets the top 10 makelaars with the most house sale offers in Amsterdam
        /// </summary>
        /// <returns></returns>
        [HttpGet("offer-count-amsterdam")]
        public async Task<IActionResult> GetMakelaarOfferCountAmsterdam()
        {
            return await GetMakelaarOfferCount("/amsterdam", 10);
        }
        
        /// <summary>
        /// Gets the top 10 makelaars with the most house sale offers in Amsterdam with a garden
        /// </summary>
        /// <returns></returns>
        [HttpGet("offer-count-amsterdam-tuin")]
        public async Task<IActionResult> GetMakelaarOfferCountAmsterdamTuin()
        {
            return await GetMakelaarOfferCount("/amsterdam/tuin", 10);
        }
    }
}
