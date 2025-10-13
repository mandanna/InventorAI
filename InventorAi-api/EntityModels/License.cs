using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class License
{
    public int LicenseId { get; set; }

    public string LicenseKey { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public bool IsActive { get; set; }

    public int StoreId { get; set; }

    public int MaxUserCount { get; set; }

    public virtual Store Store { get; set; } = null!;
}
