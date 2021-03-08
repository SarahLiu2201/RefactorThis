using Microsoft.EntityFrameworkCore;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public class ProductRepository : RepositoryCommon<ProductDbContext, Product, Guid>, IProductRepository
    {
        public ProductRepository(ProductDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<Product>> GetProducts()
        {
            return await dbContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<List<Product>> GetProductsByName(string name)
        {
            return await dbContext.Products.AsNoTracking().Where(p => p.Name.ToLower().Equals(name.ToLower())).ToListAsync();
        }

        public async Task<int> AddOrUpdateAsync(Product prod, bool isNew)
        {
            if (isNew)
            {
                dbContext.Set<Product>().Add(prod);
            }
            else
            {
                dbContext.Set<Product>().Update(prod);
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Product prod)
        {
            dbContext.Set<Product>().Remove(prod);
            return await dbContext.SaveChangesAsync();
        }
    }
}
