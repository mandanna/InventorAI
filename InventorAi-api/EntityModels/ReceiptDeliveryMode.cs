using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class ReceiptDeliveryMode
{
    public int DeliveryModeId { get; set; }

    public string ModeName { get; set; } = null!;

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}
