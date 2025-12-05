using System;
using System.Security.Cryptography;
using System.Text;

namespace MyApiProject.Helpers // Ensure this namespace matches your project structure
{
    // A robust utility class for securely hashing and verifying passwords using PBKDF2.
    public static class PasswordHasher
    {
        // Settings for the PBKDF2 algorithm (these values match typical ASP.NET Identity standards)
        private const int SaltSize = 16;     // 128-bit salt
        private const int KeySize = 32;      // 256-bit hash key
        private const int Iterations = 10000; // Recommended minimum iterations

        // Hashes the plaintext password and returns a combined string (Salt + Hash)
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // 1. Create a random salt
            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] salt = algorithm.Salt;
                byte[] hash = algorithm.GetBytes(KeySize);

                // 2. Combine the salt and hash into a single string for storage (Base64 encoded)
                // Format: {Iterations}.{SaltBase64}.{HashBase64}
                return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
            }
        }

        // Verifies a plaintext password against a stored hash string
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            try
            {
                // 1. Parse the stored hash string to extract Iterations, Salt, and Hash
                var parts = storedHash.Split('.');
                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] hash = Convert.FromBase64String(parts[2]);

                // 2. Re-hash the provided plaintext password using the retrieved salt and iterations
                using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] inputHash = algorithm.GetBytes(KeySize);

                    // 3. Compare the newly generated hash with the stored hash byte-by-byte
                    return CryptographicOperations.FixedTimeEquals(inputHash, hash);
                }
            }
            catch
            {
                // Catch exceptions if the stored hash format is bad
                return false;
            }
        }
    }
}