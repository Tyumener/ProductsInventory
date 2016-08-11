using ProductsInventory.Models;

namespace ProductsInventory.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductsInventory.Models.ProductsInventoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ProductsInventory.Models.ProductsInventoryContext";
        }

        protected override void Seed(ProductsInventory.Models.ProductsInventoryContext context)
        {
            context.Products.AddOrUpdate(
                p => p.Id,
                new Product() { Id = 1, Name = "Product 1", LastUpdated = DateTime.Now },
                new Product() { Id = 2, Name = "Product 2", LastUpdated = DateTime.Now },
                new Product() { Id = 3, Name = "Product 3", LastUpdated = DateTime.Now }
                );
        }
    }
}
