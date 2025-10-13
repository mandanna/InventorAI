namespace InventorAi_api.Models
{
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Role { get; set; }
        public string LicenseKey { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
    }
}
