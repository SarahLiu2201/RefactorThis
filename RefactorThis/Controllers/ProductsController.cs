using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Controllers.Inner;
using RefactorThis.DataLayer;
using RefactorThis.Dto;

namespace RefactorThis.Controllers
{
    public class ProductsController : CommonController
    {        
        private readonly ProductInnerController productControllerInner;
        private readonly ProductOptionInnerController productOptionInnerController;

        public ProductsController(IProductRepository productRepository, IProductOptionRepository productOptionRepository)
        {
            productControllerInner = new ProductInnerController(productRepository);
            productOptionInnerController = new ProductOptionInnerController(productOptionRepository);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(Products), 200)]
        [HttpGet(Name = "GetProducts")]
        public async Task<Products> GetProducts(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await productControllerInner.GetProducts();
            }

            return await productControllerInner.GetProductByName(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductDto), 200)]
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<Products> GetProductById(Guid id)
        {
            return await productControllerInner.GetProductById(id);         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductOptions), 200)]
        [HttpGet("{id}/Options", Name = "GetProductOptionsByProdId")]
        public async Task<ProductOptions> GetProductOptionsByProdId(Guid id)
        {
            return await productOptionInnerController.GetProductOptionsByProductId(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProductOptionDto), 200)]
        [HttpGet("{prodId}/Options/{id}", Name = "GetProductOptionsByProdIdAndOptionId")]
        public async Task<ProductOptions> GetProductOptionsByProdIdAndOptionId(Guid prodId, Guid id)
        {
            return await productOptionInnerController.GetProductOptionsByProdIdAndOptionId(prodId, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 201)]
        [HttpPost(Name = "AddProduct")]        
        public async Task<int> AddProduct(ProductDto productDto)
        {
             return await productControllerInner.AddProduct(productDto);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="Description"></param>
        /// <param name="price"></param>
        /// <param name="deliveryPrice"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 201)]
        [HttpPut("{id}", Name = "UpdateProduct")]
        public async Task<int> UpdateProduct(Guid id, ProductDto productDto)
        {
            if (id != productDto.Id)
                throw new HttpResponseException{
                    Status = 500,
                    Value = $"{id} doesn't match the value in the objet {productDto.Id}"
                };

            return await productControllerInner.UpdateProduct(productDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 201)]
        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<int> DeleteProduct(Guid id)
        {
            var productOptions = productOptionInnerController.GetProductOptionsByProductId(id);

            if (productOptions != null && productOptions.Result.Items.Count > 0)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product Options exists for Product with id {id}"
                };

            return await productControllerInner.DeleteProduct(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productOptionDto"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 201)]
        [HttpPost("{id}/Options", Name = "AddProductOptionsForProdId")]
        public async Task<int> AddProductOptionsForProdId(ProductOptionDto productOptionDto)
        {
            var product = productControllerInner.GetProductById(productOptionDto.ProductId);

            if (product == null || product.Result.Items.Count == 0)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product with id {productOptionDto.ProductId} doesn't exist"
                };

            return await productOptionInnerController.AddProductOption(productOptionDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optionId"></param>
        /// <param name="productOptionDto"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 201)]
        [HttpPut("{id}/Options/{optionId}", Name = "UpdateProductOptionsForProdId")]
        public async Task<int> UpdateProductOptionsForProdId(Guid id, Guid optionId, ProductOptionDto productOptionDto)
        {
            if (id != productOptionDto.ProductId)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"id {id} doesn't match the productId {productOptionDto.ProductId} of the product option"
                };

            if (optionId != productOptionDto.Id)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"optionId {optionId} doesn't match the id {productOptionDto.Id} of the product option"
                };

            var product = productControllerInner.GetProductById(productOptionDto.ProductId);

            if (product == null || product.Result.Items.Count == 0)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product with id {productOptionDto.ProductId} doesn't exist"
                };

            return await productOptionInnerController.UpdateProductOption(productOptionDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optionId"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(int), 201)]
        [HttpDelete("{id}/Options/{optionId}", Name = "DeleteProductOptionsForProdId")]
        public async Task<int> DeleteProductOptionsForProdId(Guid id, Guid optionId)
        {
            var product = productControllerInner.GetProductById(id);

            if (product == null || product.Result.Items.Count == 0)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product with id {id} doesn't exist"
                };

            var productOptions = productOptionInnerController.GetProductOptionById(optionId);

            if (productOptions == null || productOptions.Result.Items.Count == 0)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product Option with id {optionId} doesn't exist"
                };

            return await productOptionInnerController.DeleteProductOption(optionId);
        }
    }
}