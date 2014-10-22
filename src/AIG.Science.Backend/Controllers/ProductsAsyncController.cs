using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace AIG.Science.Backend.Controllers
{
    public class ProductsAsyncController : ApiController
    {
        // GET: api/Products
        public async Task<IEnumerable<Models.Product>> GetProducts()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NORTHWNDEntitiesAsync"].ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [dbo].[Products]";

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    var result = new List<Models.Product>();
                    while (await reader.ReadAsync())
                        result.Add(await ReadProductAsync(reader));

                    return result;
                }
            }
        }

        // GET: api/Products/5
        [ResponseType(typeof(Models.Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NORTHWNDEntitiesAsync"].ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [dbo].[Products] where ProductId=@id";
                    command.Parameters.AddWithValue("id", id);

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    var result = ReadProductAsync(reader).Result;
                    if (result != null)
                        return Ok(result);
                    else
                        return NotFound();
                }
            }
        }

        async Task<Models.Product> ReadProductAsync(SqlDataReader reader)
        {
            if (await reader.ReadAsync())
            {
                var result = new Models.Product
                {
                    ProductID = (int)reader["ProductID"],
                    ProductName = reader["ProductName"] as string,
                    UnitPrice = reader["UnitPrice"] as decimal?,
                    UnitsInStock = reader["UnitsInStock"] as short?
                };
                return result;
            }
            return null;
        }
    }
}