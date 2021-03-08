using System;
using System.Collections.Generic;
using System.Text;

namespace RefactorThis.Dto
{
    public class Products
    {
        public List<ProductDto> Items { get; set; } = new List<ProductDto>();
    }
}
