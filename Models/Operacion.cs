using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Models
{
    public class Operacion
    {
        public int Id { get; set; }

        [Required]
        public int TarjetaId { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [MaxLength(10)]
        public string TipoOperacion { get; set; } // "Balance" o "Retiro"

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Monto { get; set; } // Nullable para operaciones de balance

        // Relación con Tarjeta
        public Tarjeta Tarjeta { get; set; }
    }
}
