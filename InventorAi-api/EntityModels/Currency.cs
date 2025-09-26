using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Currency
{
    public string CurrencyCode { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public string? Symbol { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
