namespace InventorAi_api.Helpers
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt) {
            using var sha = new System.Security.Cryptography.HMACSHA512();
            passwordsalt=sha.Key;
            passwordhash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedsalt) 
        {
            
            using var sha=new System.Security.Cryptography.HMACSHA512(storedsalt);
            var computed = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(storedHash);
        }
    }
}
