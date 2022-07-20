using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using exemploDB2.Models;

#nullable disable

namespace exemploDB2.Data
{
    public partial class ExemploDB2Context : DbContext
    {
        public ExemploDB2Context()
        {
        }

        public ExemploDB2Context(DbContextOptions<ExemploDB2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Pedido> Pedidos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => new { e.PedidoId, e.ItemDescricao });

                entity.Property(e => e.PedidoId).HasMaxLength(50);

                entity.Property(e => e.ItemDescricao).HasMaxLength(50);

                entity.Property(e => e.PrecoUnitario).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
