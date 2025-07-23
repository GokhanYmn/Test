using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearchApi.Models.ECommerceModel;
using System.Collections.Immutable;

namespace ElasticSearchApi.Repo
{
    public class ECommerceRepo
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";
        public ECommerceRepo(ElasticsearchClient client)
        {
            _client = client;
        }




        public async Task<ImmutableList<ECommerce>> TermQueryAsync(string customerFirstName)
        {
            //1.Yol
            //var result=await _client.SearchAsync<ECommerce>(s=>s.Index(indexName).Query(q=>q.Term(t=>t.Field("customer_first_name.keyword").Value(customerFirstName))));

            //2.Yol
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field(f=>f.CustomerFirstName.Suffix("keyword")).Value(customerFirstName))));

            //3.Yol
            var termQuery = new TermQuery { Field = "customer_first_name.keyword", Value = customerFirstName, CaseInsensitive = true };

            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(termQuery));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> TermsQueryAsync(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirstNameList.ForEach(x =>
            {
                terms.Add(x);
            });

            //1.Yol
            //var termsQuery = new TermsQuery()
            //{
            //    Field = "customer_first_name.keyword",
            //    Terms = new TermsQueryField(terms.AsReadOnly())
            //};
            //var result=await _client.SearchAsync<ECommerce>(s=>s.Index(indexName).Query(termsQuery));


            //2.Yol
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName)
            .Size(250) //çekilen data sayısı
            .Query(q => q
            .Terms(t => t
            .Field(f => f.CustomerFirstName
            .Suffix("keyword"))
            .Terms(new TermsQueryField(terms.AsReadOnly())))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQueryAsync(string CustomerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(q => q.Prefix(p => p.Field(f => f.CustomerFullName.Suffix("keyword")).Value(CustomerFullName))));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> RangeQueryAsync(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(q => q.Range(r => r.Number(nr => nr.Field(f => f.TaxFulTotalPrice).Gte(fromPrice).Lte(toPrice)))));
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchAllQueryAsync()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Size(100).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchAllPaginationQueryAsync(int page, int pageSize)
        {
            var pagefrom = (page - 1) * pageSize;
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Size(pageSize).From(page).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> WildCardQueryAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(q => q.Wildcard(w => w.Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> FuzzyQueryAsync(string customerFirstName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s
        .Indices(indexName)
        .Query(q => q
            .Fuzzy(fu => fu
                .Field(f => f.CustomerFirstName.Suffix("keyword"))
                .Value(customerFirstName)
                         )
                     )
        .Sort(s => s
            .Field(f => f
                .Field("taxful_total_price") //  Elasticsearch field adı
                .Order(SortOrder.Desc)
                        )
                     )
             );

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(q => q.Match(m => m.Field(f => f.Category).Query(categoryName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixQueryFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(q => q.MatchBoolPrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> MatchPharesQueryFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName).Query(q => q.MatchPhrase(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleOneAsync(string cityName,double taxfulTotalPrice,string categoryNmae,string manufaturer)
        {
            var result =await _client.SearchAsync<ECommerce>(s => s.Indices(indexName)
                .Size(1000).Query(q => q.
                Bool(b => b.
                    Must(m => m.
                        Term(t => t.
                            Field("geoip.city_name")
                            .Value(cityName)))
                    .MustNot(mn => mn.
                        Range(r => r.
                            Number(n => n.
                                Field("manufacturer.keyword").Lte(taxfulTotalPrice))))
                    .Should(s => s.Term(t => t.
                        Field(f => f.Category.Suffix("keyword"))
                        .Value(categoryNmae)))
                    .Filter(f => f
                        .Term(t => t
                          .Field("manufacturer.keyword")
                          .Value(manufaturer))))
                ));
            
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> CompoundQueryExampleTwoAsync(string customerFullName)
        {
            //1. kısa yol
            var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName)
                .Size(1000).Query(q => q
                    .MatchPhrasePrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));

            //2.yol birleşik sorgu
            //var result = await _client.SearchAsync<ECommerce>(s => s.Indices(indexName)
            //    .Size(1000).Query(q => q
            //        .Bool(b => b
            //            .Should(sh => sh
            //                .Match(m => m
            //                    .Field(f => f.CustomerFullName)
            //                    .Query(customerFullName)),
            //                sh=>sh
            //                .Prefix(p => p
            //                    .Field(f => f.CustomerFullName.Suffix("keyword"))
            //                    .Value(customerFullName))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> MultiMatchQueryAsync(string name)
        {
            var result =await _client.SearchAsync<ECommerce>(s=>s.Indices(indexName)
                .Size(1000).Query(q=>q
                    .MultiMatch(mm=>mm
                    .Fields(new Field("customer_first_name")
                    .And(new Field("customer_last_name"))
                    .And(new Field("customer_full_name")))
                    .Query(name))));
            foreach (var hit in result.Hits)hit.Source.Id=hit.Id;
            return result.Documents.ToImmutableList();
                    
        }
    }
}
