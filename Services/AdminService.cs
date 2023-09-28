using System;
using System.Linq;
using System.Web.Helpers;
using Models;
using Services;

namespace Services {
    public interface IAdminService {
        public bool isAdminAlreadyRegistered(String username);
        public bool archiveExperience(String link, String username);
        public bool unArchiveExperience(String link, String username);
        public bool deleteUser(String username);
        public bool authAdmin(String username, String password);
    }
    public class AdminService : IAdminService {
        public DataContext Context { get; set; }
        public AdminService(DataContext context) {
            Context = context;
        }

        #region Cheks&Get
        public bool authAdmin(String username, String password)
        {
            var admin = Context.Administratori.Where(p => p.Username == username).FirstOrDefault();

            if (admin != null)
            {
                if (Crypto.VerifyHashedPassword(admin.Password, password) == false)
                {
                    return false;
                }

                return true;

            }
            return false;
        }
        public bool isAdminAlreadyRegistered(String username) {
            var admin = Context.Administratori.Where(p => p.Username == username).FirstOrDefault();

            if(admin == null) {
                return false;
            }
            return true;
        }
        #endregion
        #region Update
        public bool archiveExperience(String link, String username) {
            
            var iskustvo = Context.Iskustva.Where(p => p.Vozilo.Link == link).Where(p => p.Korisnik.Username == username).FirstOrDefault();
            if(iskustvo == null) {
                return false;
            }
            iskustvo.Arhivirano = true;
            Context.Iskustva.Update(iskustvo);
            Context.SaveChanges();
            return true;
        }
        public bool unArchiveExperience(String link, String username) {
            
            var iskustvo = Context.Iskustva.Where(p => p.Vozilo.Link == link).Where(p => p.Arhivirano == true).Where(p => p.Korisnik.Username == username).FirstOrDefault();
            if(iskustvo == null) {
                return false;
            }
            iskustvo.Arhivirano = false;
            Context.Iskustva.Update(iskustvo);
            Context.SaveChanges();
            return true;
        }
        #endregion
        #region Delete
        public bool deleteUser(String username) {
           
           var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).FirstOrDefault();
           var iskustva = Context.Iskustva.Where(p => p.Korisnik == user).ToList();
           if(user == null) {
               return false;
           }
           foreach(Iskustvo i in iskustva) {
               Context.Iskustva.Remove(i);
           }
           Context.RegistrovaniKorisnici.Remove(user);
           Context.SaveChanges();
           return true;
        }
        #endregion
    }                  
}                                         
