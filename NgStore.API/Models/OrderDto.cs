using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
