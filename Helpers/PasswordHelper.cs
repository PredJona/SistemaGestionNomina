using System;
using System.Security.Cryptography;
using System.Text;

namespace SistemaGestionNomina.Helpers
{
    public static class PasswordHelper
    {
        private const string Prefix = "pbkdf2-sha256$v1";
        private const int Iterations = 120000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña es obligatoria.", "password");
            }

            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            byte[] hash = Derive(password, salt, Iterations, HashSize);
            return Prefix + "$" + Iterations + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool IsLegacyHash(string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash) || storedHash.Length != 64)
            {
                return false;
            }

            for (int i = 0; i < storedHash.Length; i++)
            {
                if (!Uri.IsHexDigit(storedHash[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Verify(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }

            if (IsLegacyHash(hash))
            {
                return FixedTimeEquals(ComputeLegacyHash(password), hash.ToLowerInvariant());
            }

            string[] parts = hash.Split('$');
            if (parts.Length != 5 || !string.Equals(parts[0] + "$" + parts[1], Prefix, StringComparison.Ordinal))
            {
                return false;
            }

            int iterations;
            byte[] salt;
            byte[] expected;
            if (!int.TryParse(parts[2], out iterations) || iterations < 10000 || iterations > 1000000)
            {
                return false;
            }

            try
            {
                salt = Convert.FromBase64String(parts[3]);
                expected = Convert.FromBase64String(parts[4]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (salt.Length < SaltSize || expected.Length != HashSize)
            {
                return false;
            }

            byte[] actual = Derive(password, salt, iterations, expected.Length);
            return FixedTimeEquals(actual, expected);
        }

        private static byte[] Derive(string password, byte[] salt, int iterations, int length)
        {
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(
                password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return derive.GetBytes(length);
            }
        }

        private static string ComputeLegacyHash(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder(bytes.Length * 2);
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private static bool FixedTimeEquals(string left, string right)
        {
            return FixedTimeEquals(Encoding.UTF8.GetBytes(left ?? string.Empty), Encoding.UTF8.GetBytes(right ?? string.Empty));
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            int difference = 0;
            for (int i = 0; i < left.Length; i++)
            {
                difference |= left[i] ^ right[i];
            }

            return difference == 0;
        }
    }
}
