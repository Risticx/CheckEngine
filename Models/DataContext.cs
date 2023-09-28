using Microsoft.EntityFrameworkCore;

namespace Models 
{
    public class DataContext : DbContext 
    {
        public DbSet<Administrator> Administratori { get; set; }
        public DbSet<Sesija> Sesije { get; set; }
        
        public DbSet<Iskustvo> Iskustva { get; set; }
        
        public DbSet<RegistrovaniKorisnik> RegistrovaniKorisnici { get; set; }
        
        public DbSet<Vozilo> Vozila { get; set; }

        public DataContext(DbContextOptions options) : base(options){}
    }
}