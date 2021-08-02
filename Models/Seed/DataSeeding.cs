using System;
using System.Collections.Generic;
using System.Linq;
using FistApi.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

// dotnet ef dbcontext scaffold 
// "data source=DESKTOP-3550K4L\HARMONY;database=rfid;user id=sa; password=123" 
// Microsoft.EntityFrameworkCore.SqlServer 
// -o Model 
// -c "RfidContext"

// dotnet add package Bogus
namespace FistApi.Models.seed
{
    public static class DataSeeding
    {
        public static int i = 100;
        public static string lang = "fr";
        public static ModelBuilder Profils(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profil>().HasData(new Profil[]{
                new Profil {Id = 1, Label = "Administrateur", LabelAr = "مدير"},
                new Profil {Id = 2, Label = "Superviseur", LabelAr = "مشرف"},
                new Profil {Id = 3, Label = "Point focal", LabelAr = "	المخاطب الرئيسي"},
                new Profil {Id = 4, Label = "Propriétaire", LabelAr = "مالك"},
            });

            return modelBuilder;
        }

       
        public static ModelBuilder Users(this ModelBuilder modelBuilder)
        {
            int id = 1;

            var faker = new Faker<User>(DataSeeding.lang)
                .CustomInstantiator(f => new User { Id = id++ })
                .RuleFor(o => o.Nom, f => f.Name.FirstName())
                .RuleFor(o => o.Prenom, f => f.Name.LastName())
                .RuleFor(o => o.Email, (f, u) => f.Internet.Email($"{u.Nom}{f.UniqueIndex}", u.Prenom))
                .RuleFor(o => o.Password, f => f.Internet.Password(6))
                .RuleFor(o => o.Adresse, f => f.Address.FullAddress())
                .RuleFor(o => o.Username, (f, u) => f.Internet.UserName(u.Nom, u.Prenom))
                // .RuleFor(o => o.IdOrganisme, f => f.Random.Number(1, 6))
                .RuleFor(o => o.IdProfil, f => f.Random.Number(2, 4));
            // f.Company.CompanyName()

            // var users = faker.Generate(DataSeeding.i);
            var users = new List<User>();
            users.Add(new User
            {
                Id = id++,
                Nom = "mourabit",
                Prenom = "mohamed",
                Email = "mourabit@angular.io",
                Adresse = "Temara",
                Username = "mourabit",
                Password = "123",
                IdProfil = 1
            });
            users.Add(new User
            {
                Id = id++,
                Nom = "mehdi",
                Prenom = "mehdi",
                Email = "mehdi@angular.io",
                Adresse = "Temara",
                Username = "mehdi",
                Password = "123",
                IdProfil = 2
            });

            users.Add(new User
            {
                Id = id++,
                Nom = "ahmed",
                Prenom = "ahmed",
                Email = "ahmed@angular.io",
                Adresse = "Temara",
                Username = "ahmed",
                Password = "123",
                IdProfil = 4
            });
            users.Add(new User
            {
                Id = id++,
                Nom = "soufiane",
                Prenom = "soufiane",
                Email = "soufiane@angular.io",
                Adresse = "Temara",
                Username = "soufiane",
                Password = "123",
                IdProfil = 3
            });

            modelBuilder.Entity<User>().HasData(users);

            return modelBuilder;
        }

        

    }
}