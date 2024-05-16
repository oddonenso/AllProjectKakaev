using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Data
{
    [Table("Gender")]

    public class Gender
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GenderID")]
        public int GenderID { get; set; }

        [Required]
        [Column("GenderName")]
        [StringLength(30)]
        public string GenderName { get; set; } = string.Empty;
    }
}
