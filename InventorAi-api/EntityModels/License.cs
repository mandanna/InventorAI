using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class License
{
    public int LicenseId { get; set; }

    public int UserId { get; set; }

    public string LicenseKey { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User User { get; set; } = null!;
}
