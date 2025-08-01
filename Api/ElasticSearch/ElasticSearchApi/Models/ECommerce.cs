﻿using System.Text.Json.Serialization;

namespace ElasticSearchApi.Models.ECommerceModel
{
    public class ECommerce
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("customer_first_name")]
        public string CustomerFirstName { get; set; } = null!;
        [JsonPropertyName("customer_last_name")]
        public string CustomerLastName { get; set; }=null!;
        [JsonPropertyName("customer_full_name")]
        public string CustomerFullName { get; set; } = null!;
        [JsonPropertyName("category")]
        public string[] Category { get; set; } = null!;
        [JsonPropertyName("order_id")]
        public int OrderId { get; set; }
        [JsonPropertyName("order_date")]
        public DateTime OrderDate {  get; set; }
        [JsonPropertyName("products")]
        public Product[] Products { get; set; }= null!;
        [JsonPropertyName("tax_ful_total_price")]
        public double TaxFulTotalPrice { get; set; }

    }

    public class Product
    {
        [JsonPropertyName("product_id")]
        public long ProductId { get; set; }
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
    }
}
