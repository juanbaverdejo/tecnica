using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;

namespace PruebaTecnica.datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones ) : base(opciones)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad Tarjeta
            modelBuilder.Entity<Tarjeta>(entity =>
            {
                entity.HasIndex(t => t.NumeroTarjeta)
                      .IsUnique();

                entity.Property(t => t.NumeroTarjeta)
                      .HasMaxLength(16)
                      .IsRequired();

                entity.Property(t => t.PIN)
                      .HasMaxLength(4)
                      .IsRequired();

                entity.Property(t => t.Balance)
                      .HasColumnType("decimal(18, 2)");
            });

            // Configuración de la entidad Operacion
            modelBuilder.Entity<Operacion>(entity =>
            {
                // Índice en TarjetaId para mejorar consultas
                entity.HasIndex(o => o.TarjetaId);

                // Índice en FechaHora si es necesario
                entity.HasIndex(o => o.FechaHora);

                // Configuración de la relación con Tarjeta
                entity.HasOne(o => o.Tarjeta)
                      .WithMany(t => t.Operaciones)
                      .HasForeignKey(o => o.TarjetaId)
                      .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada

                // Configuración de TipoOperacion
                entity.Property(o => o.TipoOperacion)
                      .HasMaxLength(10)
                      .IsRequired();

                // Configuración de Monto
                entity.Property(o => o.Monto)
                      .HasColumnType("decimal(18, 2)");
            });
        }
        //Creo los modelos en el contexto.
        public DbSet<Tarjeta> Tarjeta { get; set; }

        public DbSet<Operacion> Operaciones { get; set; }
    }
}
