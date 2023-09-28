using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Iskustvo")]
    public class Iskustvo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public String Opis { get; set; }

        public bool Arhivirano { get; set; }


        [JsonIgnore]
        public Vozilo Vozilo { get; set; }
    
        [JsonIgnore]
        public Korisnik Korisnik { get; set; }
    }
}