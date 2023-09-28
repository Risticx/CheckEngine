using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models 
{
    [Table("RegistrovaniKorisnik")]
    public class RegistrovaniKorisnik : Korisnik 
    {
        [Required]
        public String Ime { get; set; }

        [Required]
        public String Prezime { get; set; }

        [Required]
        public String Email { get; set; }
    }
}