using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PetClinic.Data.Tables
{
    [Table("AccountSessions")]
    public class AccountSession
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        [Required]
        [StringLength(384)]
        public string SessionToken { get; set; }

        [Column]
        public DateTime TimeCreated { get; set; }

        [Column]
        public DateTime TimeLastRequest { get; set; }

        public bool IsClosed { get; set; }

        [Column]
        public DateTime TimeClosed { get; set; }

        public Account Account { get; set; }
    }
}
