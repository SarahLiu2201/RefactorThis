using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public interface IProductRepository : IRepositoryCommon<Product, Guid>
    {
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetProductsByName(string name);

        Task<int> AddOrUpdateAsync(Product product, bool isNew);
        Task<int> DeleteAsync(Product product);
    }
}
