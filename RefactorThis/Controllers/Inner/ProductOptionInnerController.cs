using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.DataLayer;
using RefactorThis.Dto;
using RefactorThis.Models;

namespace RefactorThis.Controllers.Inner
{
    internal class ProductOptionInnerController : ControllerBase
    {
        private readonly IProductOptionRepository productOptionRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productOptionRepository"></param>
        public ProductOptionInnerController(IProductOptionRepository productOptionRepository)
        {
            this.productOptionRepository = productOptionRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ProductOptions> GetProductOptions()
        {
            List<ProductOption> productOptions = await productOptionRepository.GetProductOptions();
            List<ProductOptionDto> results = new List<ProductOptionDto>();

            productOptions.ForEach(po => results.Add(GetProductOptionDto(po)));

            return new ProductOptions 
            {
                Items = results
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        public async Task<ProductOptions> GetProductOptionsByProductId(Guid prodId)
        {
            List<ProductOption> productOptions = await productOptionRepository.GetProductOptionsByProductId(prodId);
            List<ProductOptionDto> results = new List<ProductOptionDto>();

            productOptions.ForEach(po => results.Add(GetProductOptionDto(po)));

            return new ProductOptions
            {
                Items = results
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductOptions> GetProductOptionsByProdIdAndOptionId(Guid prodId, Guid id)
        {
            var result = await productOptionRepository.GetProductOptionsByProdIdAndOptionId(prodId, id);

            if (result == null)
            {
                return new ProductOptions();
               
            }

            return new ProductOptions
            { 
                Items = new List<ProductOptionDto> { GetProductOptionDto(result)}
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductOptions> GetProductOptionById(Guid id)
        {
            ProductOption result = await productOptionRepository.GetAsync(id);

            if (result == null)
            {
                return new ProductOptions();

            }

            return new ProductOptions
            {
                Items = new List<ProductOptionDto> { GetProductOptionDto(result) }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ProductOptions> GetProductOptionByName(string name)
        {
            List<ProductOption> products = await productOptionRepository.GetProductOptionsByName(name);
            List<ProductOptionDto> results = new List<ProductOptionDto>();

            products.ForEach(po => results.Add(GetProductOptionDto(po)));

            return new ProductOptions
            {
                Items = results
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productOptionDto"></param>
        /// <returns></returns>
        public async Task<int> AddProductOption(ProductOptionDto productOptionDto)
        {
            ProductOption existingProductOption = productOptionRepository.GetById(productOptionDto.Id);

            if (existingProductOption != null)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product Option with the same id {productOptionDto.Id} already exists"
                };

            return await productOptionRepository.AddOrUpdateAsync(new ProductOption
            {
                Id = productOptionDto.Id,
                ProductId = productOptionDto.ProductId,
                Name = productOptionDto.Name,
                Description = productOptionDto.Description
            }, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productOptionDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateProductOption(ProductOptionDto productOptionDto)
        {
            ProductOption existingProductOption = productOptionRepository.GetById(productOptionDto.Id);

            if (existingProductOption == null)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product Option with the id { productOptionDto.Id } doesn't exists"
                };

            existingProductOption.ProductId = productOptionDto.ProductId;
            existingProductOption.Name = productOptionDto.Name;
            existingProductOption.Description = productOptionDto.Description;
           
            return await productOptionRepository.AddOrUpdateAsync(existingProductOption, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteProductOption(Guid id)
        {
            ProductOption existingProductOption = productOptionRepository.GetById(id);

            if (existingProductOption == null)
                throw new HttpResponseException
                {
                    Status = 500,
                    Value = $"Product Option with the id { id } doesn't exists"
                };

            return await productOptionRepository.DeleteAsync(existingProductOption);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodOpt"></param>
        /// <returns></returns>
        private ProductOptionDto GetProductOptionDto(ProductOption prodOpt)
        {
            if (prodOpt == null)
            {
                return null;
            }

            return new ProductOptionDto
            {
                Id = prodOpt.Id,
                ProductId = prodOpt.ProductId,
                Name = prodOpt.Name,
                Description = prodOpt.Description               
            };
        }
    }

}
