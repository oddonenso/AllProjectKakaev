using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Column("Surname", TypeName = "varchar(50)")]
        public string Surname { get; set; } = string.Empty; 

        [Column("Name", TypeName = "varchar(50)")]
        public string ?Name { get; set; }

        [Column("Patronymic", TypeName = "varchar(50)")]
        public string Patronymic { get; set; } = string.Empty;

        [Column("Login", TypeName = "varchar(150)")]
        public string Login { get; set; } = string.Empty;

        [Column("Password", TypeName = "varchar(65535)")]
        public string Password { get; set; } = string.Empty;


        [Column("Address", TypeName = "varchar(255)")]
        public string Address { get; set; } = string.Empty;

        [Column("Phone", TypeName = "varchar(20)")]
        public string Phone { get; set; } = string.Empty;

        [Column("Email", TypeName = "varchar(255)")]
        public string Email { get; set; } = string.Empty;

        [Column("RoleID")]
        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public Role? Role { get; set; }

        [Column("GenderID")]
        [ForeignKey("Gender")]
        public int GenderId { get; set; }

        public Gender? Gender { get; set; }

        [Column("Photo", TypeName = "bytea")]
        public byte[]? Photo { get; set; }
    }
}
