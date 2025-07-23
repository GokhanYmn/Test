using ElasticSearchApi.Models;

namespace ElasticSearchApi.Dtos
{
    public record ProductUpdateDto(string Id,string Name, decimal Price, int Stock, ProductFeature Feature)
    {
    }
}
