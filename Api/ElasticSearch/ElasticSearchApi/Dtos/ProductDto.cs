using ElasticSearchApi.Models;


namespace ElasticSearchApi.Dtos
{
    public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
    {
        
        
        
    }
}
