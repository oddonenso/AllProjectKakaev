using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [Column("SupplierID")]
        public int SupplierID { get; set; }

        [Column("Name", TypeName = "varchar(50)")]
        public string? Name { get; set; }

        [Column("Address", TypeName = "varchar(100)")]
        public string? Address { get; set; }

        [Column("Phone", TypeName = "varchar(20)")]
        public string? Phone { get; set; }

        [Column("Email", TypeName = "varchar(50)")]
        public string? Email { get; set; }

        [Column("Photo", TypeName = "bytea")]
        public byte[]? Photo { get; set; }
    }
}
