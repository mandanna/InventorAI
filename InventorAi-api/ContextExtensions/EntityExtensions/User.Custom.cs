using InventorAi_api.EntityModels;

namespace InventorAi_api.EntityModels
{
    public partial class User
    {
        public virtual License License { get; set; }
        public bool HasValidLicense() =>
            License != null && License.IsActive && License.ExpiryDate > DateTime.UtcNow;
    }
}
