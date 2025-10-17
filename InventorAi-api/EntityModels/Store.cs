using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<License> Licenses { get; set; } = new List<License>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
