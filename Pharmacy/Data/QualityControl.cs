using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("QualityControl")]
    public class QualityControl
    {
        [Key]
        [Column("IdControl")]
        public int IdControl { get; set; }

        [Column("DataControl", TypeName = "date")]
        public DateTime DataControl { get; set; }

        [Column("ProductID")]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Column("ResultControl", TypeName = "varchar(50)")]
        public string ResultControl { get; set; } = string.Empty;

        public Product? Product { get; set; }
    }
}
