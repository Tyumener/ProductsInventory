using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsInventory.Models
{
    /// <summary>
    /// Product class
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identifier of a Product.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of a Product.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Photo of a Product.
        /// </summary>
        public byte[] Photo { get; set; }
        /// <summary>
        /// Price of a Product.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Date and time when a Product was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }
}