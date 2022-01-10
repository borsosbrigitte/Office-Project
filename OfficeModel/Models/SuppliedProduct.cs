using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeModel.Models
{
    public class SuppliedProduct
    {
        public int SupplierID { get; set; }
        public int ProductID { get; set; }
        public Supplier Supplier { get; set; }
        public Product Product { get; set; }
    }
}
