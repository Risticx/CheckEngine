using System;
using System.Linq;
using Models;
using System.Web.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Services {
    public interface IUsersService {
        bool isUserAlreadyRegistered(String username, String email);
        public bool isValidUser(String username, String password);
        public RegistrovaniKorisnik getUserInfo(String username);
        public bool authUser(String username, String password);
        public bool addUser(String username, String password, String ime,String prezime, String email);
        public bool addAdmin(String username, String password, String ime);
        public bool editUserInfo(String ime, String prezime, String email, String passowrd, String username);
    }
    public class UsersService : IUsersService {
        public DataContext Context { get; set; }
        public UsersService(DataContext context) {
            Context = context;
        }

        #region Checks&Get
        public bool isUserAlreadyRegistered(String username, String email) {
            var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).FirstOrDefault();
            var mail = Context.RegistrovaniKorisnici.Where(p => p.Email == email).FirstOrDefault();

            if(user == null && mail == null) {
                return false;
            }
            return true;
        }

        public bool isValidUser(String username, String password) {

            var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).FirstOrDefault();
            var admin = Context.Administratori.Where(p => p.Username == username).FirstOrDefault();
            if(user != null) {

                if(Crypto.VerifyHashedPassword(user.Password, password) == false) {
                    return false;
                }
               
                return true;
            }
            else if(admin != null) {

                if(Crypto.VerifyHashedPassword(admin.Password, password) == false) {
                    return false;
                }
                return true;
            }
            return false;
        }
        public RegistrovaniKorisnik getUserInfo(String username)
        {
            var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).First();

            return user;
        }
        #endregion
        #region Sessions
        public bool authUser(String username, String password) {
            var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).FirstOrDefault();
            var admin = Context.Administratori.Where(p => p.Username == username).FirstOrDefault();

            if(user != null) {
                if(Crypto.VerifyHashedPassword(user.Password, password) == false) {
                    return false;
                }

                return true;
                
            }

            return false;
        }
        #endregion
        #region Posts&Updates
        
        public bool addAdmin(String username, String password, String ime) {
            
            Administrator admin = new Administrator
            {
                Username = username,
                Password =  Crypto.HashPassword(password),
                Ime = ime
            };
            Context.Administratori.Add(admin);
            Context.SaveChanges();

            return true;
        }
        public bool addUser(String username, String password, String ime,String prezime, String email) {
            RegistrovaniKorisnik registrovaniKorisnik = new RegistrovaniKorisnik
            {
                Username = username,
                Password =  Crypto.HashPassword(password),
                Email = email,
                Ime = ime,
                Prezime = prezime
            };
            
            Context.RegistrovaniKorisnici.Add(registrovaniKorisnik);
            Context.SaveChanges();

            return true;
        }
        
        public bool editUserInfo(String ime, String prezime, String email, String password, String username)
        {
            var user_id = Context.RegistrovaniKorisnici.Where(p => p.Username == username).Select(p => p.ID).FirstOrDefault();
            var user = Context.RegistrovaniKorisnici.Where(p => p.Username == username).First();
            var mail = Context.RegistrovaniKorisnici.Where(p => p.Email == email).FirstOrDefault();
            

            if(mail != null) {
                if(Context.RegistrovaniKorisnici.Where(p => p.ID == user_id).Select(p => p.Email).FirstOrDefault() != Context.RegistrovaniKorisnici.Where(p => p.Email == email).Select(p => p.Email).FirstOrDefault()) {
                    return false;
                }
            }
            user.Ime = ime;
            user.Prezime = prezime;
            user.Email = email;
            if(password != null) {
                user.Password = Crypto.HashPassword(password);
            }
            Context.RegistrovaniKorisnici.Update(user);
            Context.SaveChanges();

            return true;
        }
        #endregion
    }
}