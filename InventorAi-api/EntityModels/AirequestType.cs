using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class AirequestType
{
    public int RequestTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Ailog> Ailogs { get; set; } = new List<Ailog>();
}
