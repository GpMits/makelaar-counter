using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MakelaarCounter.Api.Models;
using MakelaarCounter.Api.Models.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace MakelaarCounter.Api.Services
{
    public class OfferService : IOfferService
    {
        private readonly HttpClient _client;

        private readonly IMemoryCache _memoryCache;
        
        public OfferService(HttpClient client, IMemoryCache memoryCache)
        {
            _client = client;
            _memoryCache = memoryCache;
        }
        
        private static string GetNewEntryCacheKey(string searchQuery) => $"{searchQuery}_new";
        
        private static string GetOldEntryCacheKey(string searchQuery) => $"{searchQuery}_old";

        public async Task<IEnumerable<MakelaarOfferCount>> GetMakelaarOfferCount(string searchQuery, int top)
        {
            var offers = await GetAllOffers(searchQuery);

            return Core.MakelaarCounter.GetMakelaarCount(offers, top);
        }

        /// <summary>
        /// Get all offers using a search query.
        /// It first tries to take the "non persistent" (with expiration) entry in the memory cache,
        /// if it fails, it tries to get the "persistent" (without expiration) entry in the memory cache.
        /// If it fails getting the values from the cache, it will get from the Aanbod service.
        /// </summary>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Offer>> GetAllOffers(string searchQuery)
        {
            var newCacheKey = GetNewEntryCacheKey(searchQuery);
            var oldCacheKey = GetOldEntryCacheKey(searchQuery);
            
            if (_memoryCache.TryGetValue(newCacheKey, out IEnumerable<Offer> newOffers))
            {
                return newOffers;
            }
            
            if (!_memoryCache.TryGetValue(oldCacheKey, out IEnumerable<Offer> oldOffers))
            {
                newOffers = await GetAndSetNewOffersCache(searchQuery);
                return newOffers;
            }

            GetAndSetNewOffersCache(searchQuery);
            return oldOffers;
        }

        private async Task<IEnumerable<Offer>> GetAndSetNewOffersCache(string searchQuery)
        {
            IEnumerable<Offer> offers = new List<Offer>();

            try
            {
                offers = await GetAllOffersFromApi(searchQuery);
                SetNewOffersCache(offers, searchQuery);
                return offers;
            }
            catch (TaskCanceledException)
            {
                // Log error
                return offers;
            }
        }

        private void SetNewOffersCache(IEnumerable<Offer> offers, string searchQuery)
        {
            _memoryCache.Set(
                GetNewEntryCacheKey(searchQuery),
                offers,
                TimeSpan.FromMinutes(1));
            SetOldOffersCache(offers, searchQuery);
        }
        
        private void SetOldOffersCache(IEnumerable<Offer> offers, string searchQuery)
        {
            _memoryCache.Set(
                GetOldEntryCacheKey(searchQuery), offers);
        }

        private async Task<IEnumerable<Offer>> GetAllOffersFromApi(string searchQuery)
        {
            var firstPage = await GetOffersPage(searchQuery, 1);

            var tasks = new List<Task<GetOffersResponse>>();
            for (var page = 2; page <= firstPage.Paging.AantalPaginas; page++)
            {
                tasks.Add(GetOffersPage(searchQuery, page));
            }

            var getOfferResponses = await Task.WhenAll(tasks);
            
            return getOfferResponses
                .SelectMany(c => c.Objects)
                .Concat(firstPage.Objects);
        }

        private async Task<GetOffersResponse> GetOffersPage(string searchQuery, int pageNumber)
        {
            return await _client.GetFromJsonAsync<GetOffersResponse>(
                $"?type=koop" +
                $"&zo={searchQuery}" +
                $"&page={pageNumber}" +
                "&pageSize=25");
        }
        
    }
}