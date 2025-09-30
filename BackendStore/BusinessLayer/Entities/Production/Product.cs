using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Production
{
    public class Product
    {
        public int Productid { get; set; }
        public string Productname { get; set; } = string.Empty;
    }
}
