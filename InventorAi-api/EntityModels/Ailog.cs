using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Ailog
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public int RequestTypeId { get; set; }

    public string? RequestPayload { get; set; }

    public string? Response { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual AirequestType RequestType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
