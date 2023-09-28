using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models 
{
    public abstract class Korisnik 
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public String Username { get; set; }

        [Required]
        public String Password { get; set;}

        [JsonIgnore]
        public List<Iskustvo> Iskustva { get; set; }
    }
}