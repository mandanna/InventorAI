using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class PaymentType
{
    public int PaymentTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
