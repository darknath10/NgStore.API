using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Models
{
    public class OrderItemPostDto
    {
        [Required]
        public int ProductId { get; set; }

        public decimal? UnitPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
