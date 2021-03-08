using RefactorThis.DataLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Dto;
using RefactorThis.Models;

namespace RefactorThis.Controllers.Inner
{
    internal class ProductInnerController : ControllerBase
    {
        private IProductRepository productRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productRepository"></param>
        public ProductInnerController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Products> GetProducts()
        {
            List<Product> products = await productRepository.GetProducts();
            List<ProductDto> results = new List<ProductDto>();

            products.ForEach(p => results.Add(GetProductDto(p)));

            return new Products
            {
                Items = results
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Products> GetProductById(Guid id)
        {
            var product = await productRepository.GetAsync(id);          
            
            if (product == null)
            {
                return new Products();
            }

            return new Products
            {
                Items = new List<ProductDto> { GetProductDto(product) }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Products> GetProductByName(string name)
        {
            var products = await productRepository.GetProductsByName(name);
            List<ProductDto> results = new List<ProductDto>();

            products.ForEach(p => results.Add(GetProductDto(p)));

            return new Products
            {
                Items = results
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<int> AddProduct(ProductDto productDto)
        {
            Product existingProduct = productRepository.GetById(productDto.Id);

            if (existingProduct != null)
                throw new HttpResponseException 
                {
                    Status = 500,
                    Value = $"Product with the same id {productDto.Id} already exists"
                };

            return await productRepository.AddOrUpdateAsync(new Product
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                DeliveryPrice = productDto.DeliveryPrice
            }, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateProduct(ProductDto productDto)
        {
            Product existingProduct = productRepository.GetById(productDto.Id);
                       
            if (existingProduct == null)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"No product found with the id {productDto.Id}"
                };

            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Price = productDto.Price;
            existingProduct.DeliveryPrice = productDto.DeliveryPrice;

            return await productRepository.AddOrUpdateAsync(existingProduct, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteProduct(Guid id)
        {
            Product existingProduct = productRepository.GetById(id);

            if (existingProduct == null)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"No product found with the id {id}"
                };

            return await productRepository.DeleteAsync(existingProduct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        private ProductDto GetProductDto(Product prod)
        {
            if (prod == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                Price = prod.Price,
                DeliveryPrice = prod.DeliveryPrice
            };          
        }
    }
}
