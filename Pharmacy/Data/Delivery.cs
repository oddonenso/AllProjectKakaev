using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Deliveries")]
    public class Delivery
    {
        [Key]
        [Column("DeliveryID")]
        public int DeliveryID { get; set; }

        [Column("DeliveryDate", TypeName = "date")]
        public DateTime DeliveryDate { get; set; }


        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("Price", TypeName = "numeric(10, 2)")]
        public decimal Price { get; set; }

        [Column("SupplierID")]
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }

        [Column("PickupRequestID")]
        [ForeignKey("PickupRequest")]
        public int PickupRequestID { get; set; }


        [Column("Photo", TypeName = "bytea")]
        public byte[]? Photo { get; set; }


        public PickupRequest? PickupRequest { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
