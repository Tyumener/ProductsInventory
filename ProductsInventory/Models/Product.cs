using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsInventory.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}