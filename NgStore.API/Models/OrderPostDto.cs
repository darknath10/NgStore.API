using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NgStore.API.Models
{
    public class OrderPostDto : IValidatableObject
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public ICollection<OrderItemPostDto> OrderItems { get; set; }

        public bool Discount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext valContext)
        {
            foreach (var item in OrderItems)
            {                
                if (OrderItems.Count(oi => oi.ProductId == item.ProductId) > 1)
                {
                    yield return new ValidationResult("Each product can be chosen just once.");
                }
            }
        }
    }
}
