using ElasticSearch.Web.Repo;
using ElasticSearch.Web.ViewModel;

namespace ElasticSearch.Web.Services
{
    public class ECommerService
    {
        private readonly ECommerRepo _eCommerRepo;

        public ECommerService(ECommerRepo eCommerRepo)
        {
            _eCommerRepo = eCommerRepo;
        }

        public async Task<(List<ECommerceViewModel>, long totalCount, long pageLinkCount)> SearchAsync(ECommerceSearchViewModel searchModel, int page, int pageSize)
        {
            var (eCommerceList, totalCount) = await _eCommerRepo.SearchAsync(searchModel, page, pageSize);
            var pageLinkCountCalculate = totalCount % pageSize;
            long pageLinkCount = 0;

            if (pageLinkCountCalculate == 0)
            {
                pageLinkCount = totalCount / pageSize;
            }
            else
            {
                pageLinkCount = (totalCount / pageSize) + 1;
            }

            var eCommerceListViewModel = eCommerceList.Select(x => new ECommerceViewModel()
            {
                Category = String.Join(",", x.Category),
                CustomerFullName = x.CustomerFullName,
                CustomerFirstName = x.CustomerFirstName,
                CustomerLastName = x.CustomerLastName,
                Gender = x.Gender,
                OrderDate = x.OrderDate.ToShortDateString(),
                Id = x.Id,
                OrderId = x.OrderId,
                TaxFulTotalPrice = x.TaxFulTotalPrice,
            }).ToList();

            return (eCommerceListViewModel, totalCount, pageLinkCount);
        }
    }
}
