using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataKendaraan.DataModels
{
    public partial class DB_KendaraanAppContext : DbContext
    {
        public DB_KendaraanAppContext()
        {
        }

        public DB_KendaraanAppContext(DbContextOptions<DB_KendaraanAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblDataKendaraan> DataKendaraans { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;initial Catalog=DB_KendaraanApp;user id=sa; Password=P@ssw0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblDataKendaraan>(entity =>
            {
                entity.HasKey(e => e.NoRegistrasi)
                    .HasName("PK__Data_Ken__2BC3D1C3FA7488F0");

                entity.Property(e => e.No).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
