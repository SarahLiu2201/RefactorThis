using Microsoft.EntityFrameworkCore;
using RefactorThis.Models;

namespace RefactorThis.DataLayer
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {           
        }     

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }
    }
}
