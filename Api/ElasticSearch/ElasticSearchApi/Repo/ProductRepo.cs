using ElasticSearchApi.Dtos;
using ElasticSearchApi.Models;
using Nest;
using System.Collections.Immutable;

namespace ElasticSearchApi.Repo
{
    public class ProductRepo
    {
        private readonly ElasticClient _client;
        private const string indexName = "products";
        public ProductRepo(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;
            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));
            if (response.IsValid) return null;
            newProduct.Id = response.Id;
            return newProduct;
        }
        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(
                s => s.Index(indexName).Query(q => q.MatchAll()));

            foreach(var hit in result.Hits) hit.Source.Id=hit.Id;

             return result.Documents.ToImmutableList();
        }
        public async Task<Product?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));
            if (response.IsValid)
            {
                return null;
            }
            response.Source.Id = response.Id;
            return response.Source;
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateDto)
        {
            var response=await _client.UpdateAsync<Product,ProductUpdateDto>(updateDto.Id,x=>x.Index(indexName).Doc(updateDto));
            return response.IsValid;
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response=await _client.DeleteAsync<Product>(id,x=>x.Index(indexName));
            return response;
        }
    }
}
