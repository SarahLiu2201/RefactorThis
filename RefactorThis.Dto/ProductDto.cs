using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Dto
{
    [Serializable]

    public class ProductDto
    {       
        [JsonProperty]
        public Guid Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public decimal Price { get; set; }
        [JsonProperty]
        public decimal DeliveryPrice { get; set; }        
    }
}
