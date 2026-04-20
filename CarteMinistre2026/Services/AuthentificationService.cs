using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CarteMinistre2026.Data;
using CarteMinistre2026.Models;

namespace CarteMinistre2026.Services
{
    public class AuthenticationService
    {
        public bool Login(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username);
                if (user == null)
                    return false;

                string hashed = HashPassword(password);
                return user.PasswordHash == hashed;
            }
        }

        public void CreateAdminUser(string username, string password)
        {
            using (var db = new AppDbContext())
            {
                if (db.Users.Any(u => u.Username == username))
                    return;

                var user = new User
                {
                    Username = username,
                    PasswordHash = HashPassword(password)
                };
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}