using cityWatch_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace cityWatch_Project.Helpers
{
    public class PasswordHasher
    {
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public string Hash(User user, string password) => _hasher.HashPassword(user, password);
        //meke me samnya functon definition ek nathuwa lambda based function definition ekk daala thiynne

        public bool verify(User user, string paswword) 
        {
            return _hasher.VerifyHashedPassword(user, user.PasswordHash, paswword) == PasswordVerificationResult.Success;
        }

    }
}
