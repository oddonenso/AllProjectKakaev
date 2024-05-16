using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RoleID")]
        public int RoleID { get; set; }

        [Required]
        [Column("RoleName")]
        [StringLength(30)]
        public string RoleName { get; set; } = string.Empty;
    }
}
