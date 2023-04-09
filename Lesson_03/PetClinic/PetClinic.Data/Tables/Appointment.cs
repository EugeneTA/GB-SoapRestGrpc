using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Data.Tables
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [ForeignKey(nameof(Pet))]
        public int PetId { get; set; }

        [Column]
        public DateTime Date { get; set; }

        [Column]
        public string? Complaint { get; set; }

        public Client Client { get; set; }

        public Pet Pet { get; set; }
    }
}
