using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public int? StoreId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Store? Store { get; set; }
}
