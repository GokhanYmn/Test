using ElasticSearch.Web.Models;
using ElasticSearch.Web.Services;
using ElasticSearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.Web.Controllers
{
    public class BlogController : Controller
    {
        private BlogServices _blogService;

        public BlogController(BlogServices blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Search()
        {
            return View(await _blogService.SearchAsync(string.Empty));
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            ViewBag.SearchText = searchText;

            return View(await _blogService.SearchAsync(searchText));
        }
        public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            var isSuccess=await _blogService.SaveAsync(model);
            if (!isSuccess)
            {
                TempData["result"] = "kayıt başarısız";
                return RedirectToAction(nameof(BlogController.Save));
            }
            TempData["result"] = "kayıt başarılı";
            return RedirectToAction(nameof(BlogController.Save)); 
        }

    }
}
