using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.Web.Models;

namespace ElasticSearch.Web.Repo
{
    public class BlogRepo
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "blog";
        public BlogRepo(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Blog?> SaveAsync(Blog newBlog)
        {
            newBlog.Created=DateTime.Now;
            var response=await _client.IndexAsync(newBlog,x=>x.Index(indexName));

            if (!response.IsValidResponse) return null;
            newBlog.Id=response.Id;
            return newBlog;
        }

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            List<Action<QueryDescriptor<Blog>>> ListQuery = new();

            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll();
            Action<QueryDescriptor<Blog>> matchContent = (q) => q.Match(m=>m.Field(f=>f.Content).Query(searchText));
            Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(m=>m.Field(f=>f.Content).Query(searchText));

            if(string.IsNullOrEmpty(searchText))
            {
                ListQuery.Add(matchAll);
            }
            else
            {
                ListQuery.Add(matchContent);
                ListQuery.Add(titleMatchBoolPrefix);
            }

                var result = await _client.SearchAsync<Blog>(s => s.Indices(indexName)
                    .Size(1000).Query(q => q
                        .Bool(b => b
                            .Should(sh => sh
                                .Match(m => m
                                    .Field(f => f.Content)
                                    .Query(searchText)),
                                sh => sh
                                .MatchBoolPrefix(p => p
                                    .Field(f => f.Title)
                                    .Query(searchText))))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToList();
        }
    }
}
