using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Data.Entity.Infrastructure;
using ProductsInventory.Controllers;
using ProductsInventory.Models;

namespace ProductsInventory.Tests.Controllers
{
    class ProductsControllerTest
    {
        [TestMethod]
        public async Task PostProduct_ShouldCreateNewProduct()
        {
            var mockSet = new Mock<DbSet<Product>>();

            var mockContext = new Mock<ProductsInventoryContext>();
            mockContext.Setup(p => p.Products).Returns(mockSet.Object);

            var productsController = new ProductsController(mockContext.Object);
            await productsController.PostProduct(new Product() { Id = 1, Name = "Product 1" });

            mockSet.Verify(m => m.Add(It.IsAny<Product>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public void GetProducts_ShouldReturnAllProducts()
        {
            var data = GetTestProducts().AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<ProductsInventoryContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var service = new ProductsController(mockContext.Object);
            var products = service.GetProducts();

            Assert.AreEqual(2, products.Count());
        }


        [TestMethod]
        public async Task GetProduct_ShouldReturnCorrectProduct()
        {
            var testProducts = GetTestProducts();
            var data = testProducts.AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            var mockContext = new Mock<ProductsInventoryContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var service = new ProductsController(mockContext.Object);
            var result = await service.GetProduct(2) as OkNegotiatedContentResult<Product>;
            mockSet.Verify(m => m.FindAsync(2), Times.Once());
        }

        [TestMethod]
        public async Task DeleteProduct_ShouldDeleteCorrectProduct()
        {
            var testProducts = GetTestProducts();
            var data = testProducts.AsQueryable();
            var mockSet = new Mock<DbSet<Product>>();
            var mockContext = new Mock<ProductsInventoryContext>();
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);
            mockContext.Setup(c => c.Products.FindAsync(1)).Returns(Task.FromResult(testProducts[0]));

            var service = new ProductsController(mockContext.Object);
            var result = await service.DeleteProduct(1) as OkNegotiatedContentResult<Product>;
            mockContext.Verify(m => m.Products.Remove(testProducts[0]), Times.Once());
        }

        private List<Product> GetTestProducts()
        {
            var testProducts = new List<Product>();
            testProducts.Add(new Product() { Id = 1, Name = "P 1" });
            testProducts.Add(new Product() { Id = 2, Name = "P 2" });
            return testProducts;
        }
    }
}
