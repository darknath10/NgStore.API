using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Models
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public virtual ProductDto Product { get; set; }
    }
}
