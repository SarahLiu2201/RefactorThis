using System;
using System.Collections.Generic;
using System.Text;

namespace RefactorThis.Dto
{
    public class ProductOptions
    {
        public List<ProductOptionDto> Items { get; set; } = new List<ProductOptionDto>();
    }
}
