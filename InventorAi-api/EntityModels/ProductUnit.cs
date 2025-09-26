using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class ProductUnit
{
    public int UnitId { get; set; }

    public int ProductId { get; set; }

    public string UnitName { get; set; } = null!;

    public decimal ConversionFactor { get; set; }

    public string? Barcode { get; set; }

    public decimal Price { get; set; }

    public decimal CostPrice { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
