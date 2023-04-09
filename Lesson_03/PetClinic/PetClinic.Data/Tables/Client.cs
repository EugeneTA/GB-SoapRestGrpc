using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetClinic.Data.Tables
{
    [Table("Clients")]
    public class Client
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }

        [Column]
        [StringLength(50)]
        public string? Document { get; set; }

        [Column]
        [StringLength(255)]
        public string? FirstName { get; set; }

        [Column]
        [StringLength(255)]
        public string? Surname { get; set; }

        [Column]
        [StringLength(255)]
        public string? Patronymic { get; set; }

        // Клиенту может принадлежать несколько питомцен
        [InverseProperty(nameof(Pet.Client))]
        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();

        // Обращения клиента из таблицы Appointment
        [InverseProperty(nameof(Appointment.Client))]
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
