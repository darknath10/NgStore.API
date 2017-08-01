using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NgStore.API.Entities
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProductName { get; set; }

        [ForeignKey("supplier")]
        public int SupplierId { get; set; }

        public decimal? UnitPrice { get; set; }

        [MaxLength(30)]
        public string Package { get; set; }

        public bool IsDiscontinued { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
