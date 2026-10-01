using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Ecommerce.Gros
{
    public class ProductsStatsDto
    {
        public int All { get; set; }
        public int Available { get; set; }
        public int NotAvailable { get; set; }
        public int Published { get; set; }
        public int Unpublished { get; set; }
        public int Prepared { get; set; }
    }
}
