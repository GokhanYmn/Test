using ElasticSearch.Web.Services;
using ElasticSearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.Web.Controllers
{
    public class ECommerceController : Controller
    {
        private readonly ECommerService _service;

        public ECommerceController(ECommerService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Search([FromQuery]SearchPageViewModel searchPageView)
        {
            var (eCommerceList, totalCount, pageLinkCount) = await _service.SearchAsync(
                searchPageView.SearchViewModel, searchPageView.Page,
                searchPageView.PageSize);
            
            searchPageView.List=eCommerceList;
            searchPageView.TotalCount=totalCount;
            searchPageView.PageLinkCount=pageLinkCount;


            return View(searchPageView);
        }
    }
}
