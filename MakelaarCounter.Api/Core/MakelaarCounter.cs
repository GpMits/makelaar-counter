using System.Collections.Generic;
using System.Linq;
using MakelaarCounter.Api.Models;

namespace MakelaarCounter.Api.Core
{
    public static class MakelaarCounter
    {
        public static IEnumerable<MakelaarOfferCount> GetMakelaarCount(IEnumerable<Offer> offers, int amount)
        {
            return offers.GroupBy(c => c.MakelaarId)
                .Select(group => new MakelaarOfferCount
                {
                    Makelaar = new Makelaar
                    {
                        Id = group.Key,
                        Name = group.ElementAt(0).MakelaarNaam   
                    },
                    OfferCount = group.Count()
                })
                .OrderByDescending(makelaar => makelaar.OfferCount)
                .Take(amount);
        }
    }
}