using RefactorThis.DataLayer;
using RefactorThis.Models;
using System;
using Xunit;
using FluentAssertions;

namespace RefactoreThis.UnitTest
{
    public class RepositoryTest : TestBase
    {        
        [Fact]
        public void TestGetProductByIdNotFound()
        {          
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                Product p = productRepository.GetById(new Guid());
                p.Should().BeNull();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductByIdOK()
        {            
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                Product p = productRepository.GetById(products[0].Id);
                p.Should().NotBeNull();
                p.Id.Should().Equals(products[0].Id);
                p.Name.Should().Equals(products[0].Name);
                p.Price.Should().Equals(products[0].Price);
                p.DeliveryPrice.Should().Equals(products[0].DeliveryPrice);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductByNameNotFound()
        {           
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                var p = productRepository.GetProductsByName("NonExisting");
                p.Result.Should().BeEmpty();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductByNameOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                var p = productRepository.GetProductsByName(products[0].Name);
                p.Result.Should().NotBeEmpty();
                p.Result.Should().HaveCount(1);

                Product prod = p.Result[0];

                prod.Id.Should().Equals(products[0].Id);
                prod.Name.Should().Equals(products[0].Name);
                prod.Price.Should().Equals(products[0].Price);
                prod.DeliveryPrice.Should().Equals(products[0].DeliveryPrice);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestAddProductOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                Product p = new Product
                {
                    Id = new Guid(),
                    Name = "Test Product 2",
                    Description = "Test add product",
                    DeliveryPrice = new decimal(0),
                    Price = new decimal(1)
                };

                productRepository.AddOrUpdateAsync(p, true);
                var currentProducts = productRepository.GetProducts();
                currentProducts.Result.Count.Should().Be(2);
                p.Id.Should().Equals(currentProducts.Result[1].Id);
                p.Name.Should().Equals(currentProducts.Result[1].Name);
                p.Price.Should().Equals(currentProducts.Result[1].Price);
                p.DeliveryPrice.Should().Equals(currentProducts.Result[1].DeliveryPrice);
                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public void TestUpdateProductOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);

                products[0].Name = "Test Product Update 1";
                products[0].Description = "Test Product Update";
                productRepository.AddOrUpdateAsync(products[0], false);

                var currentProducts = productRepository.GetProducts();

                currentProducts.Result.Count.Should().Be(1);
                currentProducts.Result[0].Name.Should().Equals("Test Product Update 1");
                currentProducts.Result[0].Description.Should().Equals("Test Product Update");
                context.Database.EnsureDeleted();
            }
        }        

        [Fact]
        public void TestDeleteProductOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);    
                productRepository.DeleteAsync(products[0]);

                var currentProducts = productRepository.GetProducts();

                currentProducts.Result.Should().BeEmpty();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductOptionByIdOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                ProductOption po = productOptionRepository.GetById(productOptions[0].Id);
                po.Should().NotBeNull();
                po.Id.Should().Equals(productOptions[0].Id);
                po.Name.Should().Equals(productOptions[0].Name);
                po.Description.Should().Equals(productOptions[0].Description);
                po.ProductId.Should().Equals(productOptions[0].ProductId);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductOptionByIdNotFound()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                ProductOption po = productOptionRepository.GetById(new Guid());
                po.Should().BeNull();                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductOptionByProductIdNotFound()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                var p = productOptionRepository.GetProductOptionsByProductId(new Guid());
                p.Result.Should().BeEmpty();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestGetProductOptionByProductIdOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                var po = productOptionRepository.GetProductOptionsByProductId(products[0].Id);
                po.Result.Should().NotBeEmpty();
                po.Result.Should().HaveCount(1);

                ProductOption prodOpt = po.Result[0];

                prodOpt.Id.Should().Equals(productOptions[0].Id);
                prodOpt.Name.Should().Equals(productOptions[0].Name);
                prodOpt.Description.Should().Equals(productOptions[0].Description);
                prodOpt.ProductId.Should().Equals(products[0].Id);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestAddProductOptionOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                ProductOption p = new ProductOption
                {
                    Id = new Guid(),
                    ProductId = products[0].Id,
                    Name = "Test add Product Option 2",
                    Description = "Test add product option"
                };

                productOptionRepository.AddOrUpdateAsync(p, true);
                var currentProductOptions = productOptionRepository.GetProductOptions();
                currentProductOptions.Result.Count.Should().Be(2);

                p.ProductId.Should().Equals(currentProductOptions.Result[1].ProductId);
                p.Name.Should().Equals(currentProductOptions.Result[1].Name);                
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestUpdateProductOptionOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);

                productOptions[0].Name = "Test Update Product Option";
                productOptions[0].Description = "Test Update product option";                

                productOptionRepository.AddOrUpdateAsync(productOptions[0], false);
                var currentProductOptions = productOptionRepository.GetProductOptions();

                currentProductOptions.Result.Count.Should().Be(1);
                currentProductOptions.Result[0].Name.Should().Equals("Test Update Product Option");
                currentProductOptions.Result[0].Description.Should().Equals("Test Update product option");
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestDeleteProductOptionOK()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);               

                productOptionRepository.DeleteAsync(productOptions[0]);
                var currentProductOptions = productOptionRepository.GetProductOptions();
                currentProductOptions.Result.Should().BeEmpty();
                context.Database.EnsureDeleted();
            }
        }
    }
}
