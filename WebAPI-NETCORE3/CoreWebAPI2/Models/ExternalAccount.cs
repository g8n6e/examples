using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Models
{
    public class ExternalAccount
    {
        public int id { get; set; }

        public string ReferenceApplication { get; set; }

        public string ReferenceID { get; set; }
    }
}
