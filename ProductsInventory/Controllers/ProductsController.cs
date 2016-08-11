﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using NLog;
using ProductsInventory.Models;

namespace ProductsInventory.Controllers
{
    public class ProductsController : ApiController
    {
        private static readonly ITraceWriter tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();

        private ProductsInventoryContext db;
        
        public ProductsController()
        {
            db = new ProductsInventoryContext();
        }

        public ProductsController(ProductsInventoryContext db)
        {
            this.db = db;
        }

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {            
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Product not found");
                return NotFound();
            }
        
            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Bad Request");
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Bad Request");
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                        "Product not found");
                    return NotFound();
                }
                else
                {
                    tracer.Error(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                        "Could not update the database");
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Bad Request");
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                tracer.Warn(Request, ControllerContext.ControllerDescriptor.ControllerType.FullName,
                    "Product Not Found");
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}