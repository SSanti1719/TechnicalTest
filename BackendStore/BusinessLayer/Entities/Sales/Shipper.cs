using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Sales
{
    public class Shipper
    {
        public int ShipperId { get; set; }
        public string Companyname { get; set; } = string.Empty;
    }
}
