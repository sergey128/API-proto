using System.Net;
using System.Net.Http;
using Xunit;

namespace AIG.Science.Backend.Test
{
    public class ProductsControllerTest : TestCaseBase
    {
        [Fact]
        public void GetProduct_For_Existing_Id_Returns_OK()
        {
            using (HttpResponseMessage response = client.GetAsync("api/products/5").Result)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                string output = response.Content.ReadAsStringAsync().Result;

            }
        }

        [Fact]
        public void GetProduct_For_Non_Existing_Id_Returns_NotFound()
        {
            using (HttpResponseMessage response = client.GetAsync("api/products/-999999999").Result)
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
