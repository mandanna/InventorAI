using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Status
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
