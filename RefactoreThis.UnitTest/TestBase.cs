using Microsoft.EntityFrameworkCore;
using RefactorThis.DataLayer;
using RefactorThis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoreThis.UnitTest
{
    public class TestBase
    {
        protected List<Product> products;
        protected List<ProductOption> productOptions;
        protected DbContextOptionsBuilder<ProductDbContext> builder;

        public TestBase()
        {
            builder = new DbContextOptionsBuilder<ProductDbContext>();
            builder.UseInMemoryDatabase(":inMemory");
        }

        protected void SetUpTestData(ProductDbContext context)
        {
            products = new List<Product>()
            {
                new Product
                {
                    Id= new Guid(),
                    Name = "Test Product1",
                    Description = "This is for Unit test",
                    Price = new decimal(1.00),
                    DeliveryPrice = new decimal(0.99)
                }
            };

            context.AddRange(products);
            context.SaveChanges();

            productOptions = new List<ProductOption>()
                {
                    new ProductOption
                    {
                        Id= new Guid(),
                        ProductId = products[0].Id,
                        Name = "Test Product Option1",
                        Description = "This is for Unit test"
                    }

            };

            context.AddRange(productOptions);
            context.SaveChanges();
        }

    }
}
