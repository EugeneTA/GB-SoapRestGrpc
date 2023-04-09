using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Data.Tables
{
    [Table("Pets")]
    public class Pet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        [Column]
        [StringLength(50)]
        public string? Name { get; set; }

        [Column]
        public DateTime Birthday { get; set; }

        public Client Client { get; set; }

        // Осмотры животного из таблицы Appointment
        [InverseProperty(nameof(Appointment.Pet))]
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
