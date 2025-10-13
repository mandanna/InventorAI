using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventorAi_api.Models
{
    public class LicenseRequest
    {
        [Required(ErrorMessage = "Store Id is needed")]
        public int StoreId { get; set; }
        [Required(ErrorMessage = "Store Id is needed")]
        [Range(1, int.MaxValue, ErrorMessage = "MaxUserCount must be at least 1.")]
        [DefaultValue(1)]
        public int MaxUserCount { get; set; }
    }
}
