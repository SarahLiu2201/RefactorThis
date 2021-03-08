using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RefactorThis.DataLayer;

namespace RefactorThis.Models
{
    [Serializable]
    [Table ("Products")]
    public class Product : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }    
    }
}