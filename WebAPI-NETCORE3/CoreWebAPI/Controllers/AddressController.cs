using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;

namespace CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        // GET: api/Address
        [HttpGet]
        public IEnumerable<Models.Address> Get()
        {
            string sqlAddressKeywords = "SELECT * FROM Verify.AddressKeywords;";
            using (var connection = new SqlConnection("Data Source=localhost;Initial Catalog=ExternalAccounts;Integrated Security=true;"))
            {
                var addressKeywords = connection.Query<Models.Address>(sqlAddressKeywords).ToList();

                Console.WriteLine(addressKeywords.Count);
                return addressKeywords;
            }
        }

        // GET: api/Address/5
        [HttpGet("{id}", Name = "Get")]
        public Models.Address Get(int id)
        {
            string sqlAddressKeyword = "SELECT * FROM Verify.AddressKeywords WHERE id = " + id + ";";
            using (var connection = new SqlConnection("Data Source=localhost;Initial Catalog=ExternalAccounts;Integrated Security=true;"))
            {
                var addressKeyword = connection.Query<Models.Address>(sqlAddressKeyword).FirstOrDefault();

                return addressKeyword;
            }
        }

        // POST: api/Address
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Address/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
