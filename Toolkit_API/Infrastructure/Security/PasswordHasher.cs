using System.Security.Cryptography;
using System.Text;
using Toolkit_API.Application.Interfaces;

namespace Toolkit_API.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int saltSize = 64;
        private const int iterations = 350000;
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        // hashes the password using pbkdf2 And a salt, iterates 350 000 times 
        public byte[] HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                saltSize
            );
            return hash;
        }
        // verifies given hash and the salt 
        public bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    iterations,
                    hashAlgorithm,
                    saltSize
            );
            // Used fixed time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
        }
    }
}
