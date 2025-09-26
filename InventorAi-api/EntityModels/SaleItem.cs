using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class SaleItem
{
    public int SaleItemId { get; set; }

    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public int? UnitId { get; set; }

    public decimal Quantity { get; set; }

    public decimal PricePerUnit { get; set; }

    public decimal CostPricePerUnit { get; set; }

    public decimal Discount { get; set; }

    public decimal Tax { get; set; }

    public decimal LineTotal { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;

    public virtual ProductUnit? Unit { get; set; }
}
