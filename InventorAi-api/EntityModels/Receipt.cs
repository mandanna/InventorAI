using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Receipt
{
    public int ReceiptId { get; set; }

    public int SaleId { get; set; }

    public string ReceiptNumber { get; set; } = null!;

    public int ReceiptDeliveryModeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ReceiptDeliveryMode ReceiptDeliveryMode { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
