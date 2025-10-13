namespace InventorAi_api.Models.DTO.Requests.License
{
    public class LicenseUpdateRequest
    {
        public int StoreId { get; set; }
        public bool IsRenew { get; set; }
        public bool IsGenerateKey { get; set; }
        public DateTime NewExpiryDate { get; set; }= DateTime.UtcNow.AddDays(30);
        public bool? IsActive { get; set; }
        public int MaxUserCount { get; set; }
    }
}
