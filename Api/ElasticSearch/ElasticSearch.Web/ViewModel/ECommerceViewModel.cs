using System.Text.Json.Serialization;

namespace ElasticSearch.Web.ViewModel
{
    public class ECommerceViewModel
    {
       
        public string Id { get; set; } = null!;
        
        public string CustomerFirstName { get; set; } = null!;
        
        public string CustomerLastName { get; set; } = null!;
       
        public string CustomerFullName { get; set; } = null!;
       
        public string Category { get; set; } = null!;
      
        public string Gender { get; set; } = null!;
      
        public int OrderId { get; set; }
        
        public string OrderDate { get; set; }
      
       
        public double TaxFulTotalPrice { get; set; }
    }
}
