using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public string Package { get; set; }
    }
}
