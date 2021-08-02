using System;
using System.Collections.Generic;

namespace FistApi.Models
{
    public partial class User
    {
        public User()
        {
            // Syntheses = new HashSet<Synthese>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        // [System.Text.Json.Serialization.JsonIgnore]
        public string Password { get; set; }
        public string Adresse { get; set; }
        public string Username { get; set; }
        public int? IdProfil { get; set; }

        // public virtual Organisme Organisme { get; set; }
        public virtual Profil Profil { get; set; }
        // [System.Text.Json.Serialization.JsonIgnore]
        // public virtual ICollection<Synthese> Syntheses { get; set; }
    }
}
