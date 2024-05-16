using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("PickupRequests")]
    public class PickupRequest
    {
        [Key]
        [Column("RequestID")]
        public int RequestID { get; set; }

        [Column("ProductID")]
        [ForeignKey("Product")]

        public int ProductID { get; set; }
        [Column("WarehouseID")]
        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; }


        public Product? Product { get; set; } 

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("RequestDate")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        [Column("RequesterID")]
        [ForeignKey("User")]
        public int RequesterID { get; set; }

        public Warehouse? Warehouse { get; set; }
        public User? Requester { get; set; }

        [Column("Status")]
        public string? Status { get; set; } // например, "Pending", "Approved", "Rejected"

       
    }
}
