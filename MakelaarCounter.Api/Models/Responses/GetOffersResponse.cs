using System.Collections.Generic;

namespace MakelaarCounter.Api.Models.Responses
{
    public class GetOffersResponse
    {
        public Paging Paging { get; set; }
        public IEnumerable<Offer> Objects { get; set; }
    }
        
}