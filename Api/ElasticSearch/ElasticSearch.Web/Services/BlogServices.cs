using ElasticSearch.Web.Models;
using ElasticSearch.Web.Repo;
using ElasticSearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ElasticSearch.Web.Services
{
    public class BlogServices
    {
        private readonly BlogRepo _repository;

        public BlogServices(BlogRepo repository)
        {
            _repository = repository;
        }



        public async Task<bool> SaveAsync(BlogCreateViewModel model)
        {
            var newBlog = new Blog
            {
                Title = model.Title,
                UserId = Guid.NewGuid(),
                Content = model.Content,
                Tags = model.Tags.Split(",")
            };
            var isCreatedBlog = await _repository.SaveAsync(newBlog);
            return isCreatedBlog != null;
        }

        public async Task<List<BlogViewModel>> SearchAsync(string searchText)
        {
            var blogList = await _repository.SearchAsync(searchText);
            return blogList.Select(b => new BlogViewModel()
            {
                Id=b.Id,
                Title = b.Title,
                Content = b.Content,
                Created=b.Created.ToShortDateString(),
                UserId=b.UserId.ToString(),
                Tags=String.Join(",", b.Tags),

            }).ToList();
        }
    }
}
