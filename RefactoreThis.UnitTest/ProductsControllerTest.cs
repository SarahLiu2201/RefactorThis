using FluentAssertions;
using RefactorThis.Controllers;
using RefactorThis.DataLayer;
using RefactorThis.Dto;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RefactoreThis.UnitTest
{
    public class ProductsControllerTest : TestBase
    {
        [Fact]
        public void TestUpdateProductFail()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                ProductsController productController = new ProductsController(productRepository, null);

                Func<Task> act = async () => await productController.UpdateProduct(new Guid(), new ProductDto
                {
                    Id = new Guid()
                });

                act.Should().Throw<HttpResponseException>();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestAddProductFail()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                ProductsController productController = new ProductsController(productRepository, null);

                Func<Task> act = async () => await productController.AddProduct(new ProductDto
                {
                    Id = products[0].Id
                });

                act.Should().Throw<HttpResponseException>();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestUpdateProductOptionFail()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                ProductsController productController = new ProductsController(productRepository, productOptionRepository);

                Func<Task> act = async () => await productController.UpdateProductOptionsForProdId(new Guid(), 
                                                                                                   new Guid(), 
                                                                                                   new ProductOptionDto
                                                                                                    {
                                                                                                        Id = new Guid()
                                                                                                    });

                act.Should().Throw<HttpResponseException>();
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public void TestAddProductOptionFail()
        {
            using (var context = new ProductDbContext(builder.Options))
            {
                SetUpTestData(context);
                IProductRepository productRepository = new ProductRepository(context);
                IProductOptionRepository productOptionRepository = new ProductOptionRepository(context);
                ProductsController productController = new ProductsController(productRepository, productOptionRepository);

                Func<Task> act = async () => await productController.AddProductOptionsForProdId(new ProductOptionDto
                {
                    Id = productOptions[0].Id
                });

                act.Should().Throw<HttpResponseException>();
                context.Database.EnsureDeleted();
            }
        }

    }
}
