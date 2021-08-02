using System;
using System.Collections.Generic;

namespace FistApi.Models
{
    public partial class Profil
    {
        public Profil()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Label { get; set; }
        public string LabelAr { get; set; }
        // [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<User> User { get; set; }
    }
}
