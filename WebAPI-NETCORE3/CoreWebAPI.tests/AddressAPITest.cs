using System;
using Xunit;
using System.Threading.Tasks;
using CoreWebAPI;
using System.Net;

namespace CoreWebAPI_XUnitTests
{
    public class AddressAPITest
    {
        [Fact]
        public async Task Test_Get_All()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/address");

                response.EnsureSuccessStatusCode();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
