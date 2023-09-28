using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models 
{
    [Table("Sesija")]
    public class Sesija
    {
        [Key]
        public String SessionId { get; set; }
        
        public int UserId { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}