using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WarehouseID")]
        public int WarehouseID { get; set; }

        [Required]
        [StringLength(150)]
        [Column("WarehouseName")]
        public string WarehouseName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Column("QuantityOfGoods")]
        public int QuantityOfGoods { get; set; }

        [Column("Photo")]
        public byte[]? Photo { get; set; }
    }
}
