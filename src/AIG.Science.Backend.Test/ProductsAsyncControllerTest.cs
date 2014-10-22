using System.Net;
using System.Net.Http;
using Xunit;

namespace AIG.Science.Backend.Test
{
    public class ProductsAsyncControllerTest : TestCaseBase
    {
        [Fact]
        public void Async_GetProduct_For_Existing_Id_Returns_OK()
        {
            using (HttpResponseMessage response = client.GetAsync("api/productsAsync/5").Result)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                string output = response.Content.ReadAsStringAsync().Result;

            }
        }

        [Fact]
        public void Async_GetProduct_For_Non_Existing_Id_Returns_NotFound()
        {
            using (HttpResponseMessage response = client.GetAsync("api/productsAsync/-999999999").Result)
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }


        [Fact]
        public void Async_GetProducts_Returns_Result_Set()
        {
            using (HttpResponseMessage response = client.GetAsync("api/productsAsync").Result)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
