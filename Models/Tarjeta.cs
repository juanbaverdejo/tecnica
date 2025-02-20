using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Models
{
    public class Tarjeta
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(16)]
        public string NumeroTarjeta { get; set; }

        [Required]
        [MaxLength(4)]
        public string PIN { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }

        public bool Bloqueada { get; set; }

        public int IntentosFallidos { get; set; }

        [Required]
        public DateTime FechaVencimiento { get; set; }
        // Propiedad de navegación para las operaciones
        public ICollection<Operacion> Operaciones { get; set; } = new List<Operacion>();
    }
}
