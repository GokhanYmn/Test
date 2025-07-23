using ElasticSearchApi.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private readonly ECommerceRepo _eCommerceRepo;

        public ECommerceController(ECommerceRepo eCommerceRepo)
        {
            _eCommerceRepo = eCommerceRepo;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string customerFirstName)
        {
            return Ok(await _eCommerceRepo.TermQueryAsync(customerFirstName));
        }

        [HttpPost]
        public async Task<IActionResult> TermsQuery(List<string> customerFirstNameList)
        {
            return Ok(await _eCommerceRepo.TermsQueryAsync(customerFirstNameList));
        }
        [HttpGet]
        public async Task<IActionResult> PrefixQuery(string customerFullName)
        {
            return Ok(await _eCommerceRepo.PrefixQueryAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> RangeQuery(double fromPrice, double toPrice)
        {
            return Ok(await _eCommerceRepo.RangeQueryAsync(fromPrice, toPrice));
        }

        [HttpGet]
        public async Task<IActionResult> MatchAllQuery()
        {
            return Ok(await _eCommerceRepo.MatchAllQueryAsync());
        }
        [HttpGet]
        public async Task<IActionResult> MatchAllPaginationQuery(int page = 1, int pageSize = 10)
        {
            return Ok(await _eCommerceRepo.MatchAllPaginationQueryAsync(page, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> WildCardQuery(string customerFullName)
        {
            return Ok(await _eCommerceRepo.WildCardQueryAsync(customerFullName));
        }

        [HttpGet]
        public async Task<IActionResult> FuzzyQuery(string customerName)
        {
            return Ok(await _eCommerceRepo.FuzzyQueryAsync(customerName));
        }


        [HttpGet]
        public async Task<IActionResult> MatchQueryFullText(string category)
        {
            return Ok(await _eCommerceRepo.MatchQueryFullTextAsync(category));
        }

        [HttpGet]
        public async Task<IActionResult> MatchBoolPrefixQueryFullText(string customerName)
        {
            return Ok(await _eCommerceRepo.MatchBoolPrefixQueryFullTextAsync(customerName));
        }
        [HttpGet]
        public async Task<IActionResult> MatchPhraseQueryFullText(string customerName)
        {
            return Ok(await _eCommerceRepo.MatchPharesQueryFullTextAsync(customerName));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleOne(string cityName, double taxfulTotalPrice, string categoryNmae, string manufaturer)
        {
            return Ok(await _eCommerceRepo.CompoundQueryExampleOneAsync(cityName, taxfulTotalPrice, categoryNmae, manufaturer));
        }

        [HttpGet]
        public async Task<IActionResult> CompoundQueryExampleTwo(string customerFullName)
        {
               return Ok(await _eCommerceRepo.CompoundQueryExampleTwoAsync(customerFullName));
        }
        [HttpGet]
        public async Task<IActionResult> MultiMatchQuery(string name)
        {
            return Ok(await _eCommerceRepo.MultiMatchQueryAsync(name));
        }
    }  
}
