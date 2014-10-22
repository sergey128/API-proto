using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;

namespace AIG.Science.Backend.Controllers
{
    public class ProductsAsyncController : ApiController
    {
        NORTHWNDEntities db = new NORTHWNDEntities();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Models.Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            var query = db.Products.Where(x => x.ProductID == id);
            var resultSet = await query.ExecuteAsync<Product>();
            var product = resultSet.SingleOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            var result = new Models.Product
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock
            };
            return Ok(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}