using Newtonsoft.Json;
using System;

namespace RefactorThis.Dto
{
    public class ProductOptionDto
    {
        [JsonProperty]

        public Guid Id { get; set; }

        [JsonProperty]

        public Guid ProductId { get; set; }
        [JsonProperty]

        public string Name { get; set; }
        [JsonProperty]

        public string Description { get; set; }
    }
}
