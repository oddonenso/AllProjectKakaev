using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Sales")]
    public class Sale
    {
        [Key]
        [Column("SaleID")]
        public int SaleID { get; set; }

        [Column("SaleDate", TypeName = "date")]
        public DateTime SaleDate { get; set; }

        [Column("ProductID")]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("Price", TypeName = "numeric(10, 2)")]
        public decimal Price { get; set; }

        public Product? Product { get; set; }
    }
}
