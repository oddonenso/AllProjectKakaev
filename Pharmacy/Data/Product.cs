using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("ProductID")]
        public int ProductID { get; set; }

        [Column("Name", TypeName = "varchar(50)")]
        public string? Name { get; set; }

        [Column("Manufacturer", TypeName = "varchar(50)")]
        public string Manufacturer { get; set; } = string.Empty;

        [Column("Price", TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("ShelfLife", TypeName = "date")]
        public DateTime ShelfLife { get; set; }

        [Column("Certificate", TypeName = "varchar(50)")]
        public string Certificate { get; set; } = string.Empty;

        [Column("Photo", TypeName = "bytea")]
        public byte[]? Photo { get; set; }

        [Column("WarehouseID")]
        [ForeignKey("Warehouse")]
        public int WarehouseID { get; set; }

        public Warehouse? Warehouse { get; set; }
    }
}
