using Microsoft.EntityFrameworkCore;
using GerenciamentoHoras.Models;

namespace GerenciamentoHoras.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<RegistroHoras> RegistrosHoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RegistroHoras>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Matricula).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EXT).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Data).IsRequired();
                entity.Property(e => e.QuantidadeHoras).IsRequired().HasColumnType("decimal(5,2)");
                entity.Property(e => e.Tipo).IsRequired();
                entity.Property(e => e.DataCriacao).IsRequired();
            });
        }
    }
}

