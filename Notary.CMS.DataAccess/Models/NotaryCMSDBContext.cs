using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Notary.CMS.DataAccess.Models
{
    public partial class NotaryCMSDBContext : DbContext
    {
        public NotaryCMSDBContext()
        {
        }

        public NotaryCMSDBContext(DbContextOptions<NotaryCMSDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<Component> Components { get; set; } = null!;
        public virtual DbSet<Page> Pages { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer(onfiguration.GetConnectionString("DefaultConnection"));
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.HasIndex(e => e.Identifier, "IX_Application_Identifier")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("identifier");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.ToTable("Component");

                entity.HasIndex(e => e.ComponentIdentifier, "Unik_Component_Identifier")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComponentIdentifier)
                    .HasMaxLength(200)
                    .HasColumnName("component_identifier");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Html).HasColumnName("html");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name")
                    .IsFixedLength();

                entity.Property(e => e.PageId).HasColumnName("page_id");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.Components)
                    .HasForeignKey(d => d.PageId)
                    .HasConstraintName("FK_Component_Page");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("Page");

                entity.HasIndex(e => e.Id, "IX_Page");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(200)
                    .HasColumnName("identifier");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_date");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.Pages)
                    .HasForeignKey(d => d.AppId)
                    .HasConstraintName("FK_Page_Application");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
