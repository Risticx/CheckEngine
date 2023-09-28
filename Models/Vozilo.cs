using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Vozilo")]
    public class Vozilo 
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public String Link { get; set; }

        [Required]
        public String Marka { get; set; }

        [Required]
        public int Godiste { get; set; }

        [JsonIgnore]
        public virtual List<Iskustvo> Iskustva { get; set; }

    }

}