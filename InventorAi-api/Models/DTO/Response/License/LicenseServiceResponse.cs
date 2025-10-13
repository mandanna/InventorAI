namespace InventorAi_api.Models
{
    public class LicenseServiceResponse
    {
        public int LicenseId { get; set; }

        public string LicenseKey { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; }

        public int StoreId { get; set; }

        public int MaxUserCount { get; set; }
    }
}
