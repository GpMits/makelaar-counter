using System.Collections.Generic;
using System.Threading.Tasks;
using MakelaarCounter.Api.Models;

namespace MakelaarCounter.Api.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<MakelaarOfferCount>> GetMakelaarOfferCount(string searchQuery, int top);
        Task<IEnumerable<Offer>> GetAllOffers(string searchQuery);
    }
}