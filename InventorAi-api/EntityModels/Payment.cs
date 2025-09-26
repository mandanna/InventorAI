using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int SaleId { get; set; }

    public int PaymentTypeId { get; set; }

    public decimal Amount { get; set; }

    public string? ReferenceNumber { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Currency CurrencyCodeNavigation { get; set; } = null!;

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
