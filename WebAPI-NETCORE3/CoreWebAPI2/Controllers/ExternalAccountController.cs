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
    public class ExternalAccountsController : ControllerBase
    {
        // GET: api/Address
        [HttpGet]
        public IEnumerable<Models.ExternalAccount> Get()
        {
            string sqlExternalAccounts = "SELECT id, ReferenceApplication, ReferenceID FROM dbo.ExternalAccounts;";
            using (var connection = new SqlConnection("Data Source=D224Q72;Initial Catalog=ExternalAccounts;Integrated Security=true;"))
            {
                var externalAccounts = connection.Query<Models.ExternalAccount>(sqlExternalAccounts).ToList();

                Console.WriteLine(externalAccounts.Count);
                return externalAccounts;
            }
        }

        // GET: api/Address/5
        [HttpGet("{id}", Name = "Get")]
        public Models.ExternalAccount Get(int id)
        {
            string sqlExternalAccounts = "SELECT id, ReferenceApplication, ReferenceID FROM dbo.ExternalAccounts WHERE id = " + id + ";";
            using (var connection = new SqlConnection("Data Source=D224Q72;Initial Catalog=ExternalAccounts;Integrated Security=true;"))
            {
                var externalAccount = connection.Query<Models.ExternalAccount>(sqlExternalAccounts).FirstOrDefault();

                return externalAccount;
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
