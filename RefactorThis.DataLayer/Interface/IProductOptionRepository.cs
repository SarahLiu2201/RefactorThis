using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public interface IProductOptionRepository : IRepositoryCommon<ProductOption, Guid>
    {
        Task<List<ProductOption>> GetProductOptions();
        Task<List<ProductOption>> GetProductOptionsByName(string name);
        Task<List<ProductOption>> GetProductOptionsByProductId(Guid prodId);
        Task<ProductOption> GetProductOptionsByProdIdAndOptionId(Guid proId, Guid id);

        Task<int> AddOrUpdateAsync(ProductOption productOption, bool isNew);
        Task<int> DeleteAsync(ProductOption productOption);
    }
}
