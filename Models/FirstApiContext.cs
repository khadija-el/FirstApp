using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FistApi.Models.seed;

namespace FistApi.Models
{
    public partial class FistApiContext : DbContext
    {
        public FistApiContext()
        {
            // dotnet tool install -g dotnet-aspnet-codegenerator
            // dotnet tool update -g dotnet-aspnet-codegenerator
            // dotnet aspnet-codegenerator --project . controller -name HelloController -m Author -dc WebAPIDataContext
            // dotnet tool install --global dotnet-ef --version 3.0.0
            // scafolding to db
            // dotnet ef migrations add secondMG
            // dotnet ef database update
            // dotnet ef migrations remove
            // dotnet ef database update LastGoodMigration
            // dotnet ef migrations script
        }

        public FistApiContext(DbContextOptions<FistApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Profil> Profils { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Profil>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Label);
            });     


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Adresse).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Nom).IsRequired();
                entity.Property(e => e.Prenom).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.IdProfil).IsRequired();
                // entity.HasOne(d => d.Organisme).WithMany(p => p.User).HasForeignKey(d => d.IdOrganisme);
                entity.HasOne(d => d.Profil).WithMany(p => p.User).HasForeignKey(d => d.IdProfil);
                // entity.HasMany(d => d.Syntheses).WithOne(p => p.User).HasForeignKey(d => d.IdUser).OnDelete(DeleteBehavior.NoAction);
            });

            OnModelCreatingPartial(modelBuilder);

            modelBuilder
                .Profils()
                .Users()
             ;

            // Database.ExecuteSqlRaw("update recommendations set etat = 'Réalisé', EtatAvancementChiffre = 100 where id % 3 = 0;");
            // Database.ExecuteSqlRaw("update recommendations set etat = 'En cours', EtatAvancementChiffre = 50 where id % 3 = 1;");
            // Database.ExecuteSqlRaw("update recommendations set etat = 'Non réalisé', EtatAvancementChiffre = 0 where id % 3 = 2;");

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
