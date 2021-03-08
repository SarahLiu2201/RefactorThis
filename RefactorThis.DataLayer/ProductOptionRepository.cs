using Microsoft.EntityFrameworkCore;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.DataLayer
{
    public class ProductOptionRepository : RepositoryCommon<ProductDbContext, ProductOption, Guid>, IProductOptionRepository
    {
        public ProductOptionRepository(ProductDbContext dbContext) : base(dbContext)
        { }

        public async Task<List<ProductOption>> GetProductOptions()
        {
            return await dbContext.ProductOptions.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<ProductOption>> GetProductOptionsByName(string name)
        {
            return await dbContext.ProductOptions.AsNoTracking().Where(po => po.Name.ToLower().Equals(name.ToLower())).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        public async Task<List<ProductOption>> GetProductOptionsByProductId(Guid prodId)
        {
            var results = (from prodOpt in dbContext.ProductOptions.Cast<ProductOption>()
                           join prod in dbContext.Products on prodOpt.ProductId equals prod.Id
                           where prod.Id == prodId
                           select new ProductOption()
                           {
                               Id = prodOpt.Id,
                               ProductId = prodOpt.ProductId,
                               Description = prodOpt.Description,
                               Name = prodOpt.Name
                           });
            return await results.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductOption> GetProductOptionsByProdIdAndOptionId(Guid prodId, Guid id)
        {
            var results = (from prodOpt in dbContext.ProductOptions.Cast<ProductOption>()
                           join prod in dbContext.Products on prodOpt.ProductId equals prod.Id
                           where prod.Id == prodId && prodOpt.Id == id                          

                           select new ProductOption()
                           {
                               Id = prodOpt.Id,
                               ProductId = prodOpt.ProductId,
                               Description = prodOpt.Description,
                               Name = prodOpt.Name
                           });
            return await results.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodOpt"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(ProductOption prodOpt, bool isNew)
        {
            if (isNew)
            {
                dbContext.Set<ProductOption>().Add(prodOpt);
            }
            else
            {
                dbContext.Set<ProductOption>().Update(prodOpt);
            }

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(ProductOption prodOpt)
        {
            dbContext.Set<ProductOption>().Remove(prodOpt);
            return await dbContext.SaveChangesAsync();
        }
    }
}
