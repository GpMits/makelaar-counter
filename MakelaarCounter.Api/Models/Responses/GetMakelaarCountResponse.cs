using System.Collections.Generic;

namespace MakelaarCounter.Api.Models.Responses
{
    public class GetMakelaarCountResponse
    {
        private IEnumerable<Makelaar> Makelaars { get; set; }
    }
}