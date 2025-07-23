using Elastic.Clients.Elasticsearch;
using ElasticSearchApi.Dtos;
using ElasticSearchApi.Models;
using ElasticSearchApi.Repo;

using System.Collections.Immutable;
using System.Net;

namespace ElasticSearchApi.Services
{
    public class ProductService
    {
        private readonly ProductRepo _productRepo;
        private readonly ILogger<ProductService> _logger;
        public ProductService(ProductRepo productRepo, ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {

            var response = await _productRepo.SaveAsync(request.CreateProduct());

            if (response == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında bir hata meydana geldi." }, HttpStatusCode.ServiceUnavailable);
            }

            return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.Created);



        }
        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepo.GetAllAsync();
            var productListDto = new List<ProductDto>();

            foreach (var x in products)
            {
                if (x.Feature is null)
                {
                    productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
                }
                else
                {
                    {
                        productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color.ToString())));
                    }
                }

            }

            return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
        }
        public async Task<ResponseDto<ProductDto>> GetByIdAysnc(string id)
        {
            var hasProduct = await _productRepo.GetByIdAsync(id);
            if (hasProduct == null)
            {
                return ResponseDto<ProductDto>.Fail("Ürün Bulunamadı", HttpStatusCode.NotFound);
            }

            return ResponseDto<ProductDto>.Success(hasProduct.CreateDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updateDto)
        {
            var isSuccess = await _productRepo.UpdateAsync(updateDto);

            if (!isSuccess)
            {
                return ResponseDto<bool>.Fail(new List<string>() { "Güncelleme esnasında bir hata meydana geldi." }, HttpStatusCode.InternalServerError);

            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }


        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var deleteResponse = await _productRepo.DeleteAsync(id);

            if (!deleteResponse.IsValidResponse && deleteResponse.Result == Result.NotFound)
            {
                return ResponseDto<bool>.Fail(new List<string>() { "Silmeye çalıştığınız ürün bulunamadı." }, HttpStatusCode.NotFound);
            }


            if (!deleteResponse.IsValidResponse)
            {
                deleteResponse.TryGetOriginalException(out Exception? exception);
                _logger.LogError(exception, deleteResponse.ElasticsearchServerError.Error.ToString());
                return ResponseDto<bool>.Fail(new List<string>() { "Silme esnasında bir hata meydana geldi." }, HttpStatusCode.InternalServerError);

            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}
