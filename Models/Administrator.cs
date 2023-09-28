using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models 
{
    [Table("Administrator")]
    public class Administrator : Korisnik 
    {
        public String Ime { get; set; }
    }
}