using System;
using System.Collections.Generic;
using System.Net.Http;
using MakelaarCounter.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace MakelaarCounter.Api.Extensions
{
    public static class OfferServiceExtensions
    {
        public static void RegisterOfferService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IOfferService, OfferService>(
                    client => client.BaseAddress = GetFundaApiUri(configuration))
                .AddPolicyHandler(GetRetryPolicy());
        }
        
        public static async void StartOfferServiceCache(this IApplicationBuilder app, IEnumerable<string> searchStrings)
        {
            var serviceProvider = app.ApplicationServices;
            var offerService = serviceProvider.GetService<IOfferService>();
            foreach (var searchString in searchStrings)
            { 
                await offerService.GetAllOffers(searchString);
            }
        }
        
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(2, _ => TimeSpan.FromMinutes(1));
        }

        private static Uri GetFundaApiUri(IConfiguration configuration)
        {
            return new Uri($"{configuration["FundaApi:BaseAddress"]}/{configuration["FundaApi:ApiKey"]}/");
        }
    }
}