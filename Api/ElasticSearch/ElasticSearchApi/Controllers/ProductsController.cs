using ElasticSearchApi.Dtos;
using ElasticSearchApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchApi.Controllers
{

    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.SaveAsync(request));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto request)
        {
            return CreateActionResult(await _productService.UpdateAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(ProductCreateDto request)
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return CreateActionResult(await _productService.GetByIdAysnc(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return CreateActionResult(await _productService.DeleteAsync(id));
        }
    }
}
