using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public int? CategoryId { get; set; }

    public decimal BasePrice { get; set; }

    public decimal CostPrice { get; set; }

    public int CurrentStock { get; set; }

    public int ReorderLevel { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public bool IsComposite { get; set; }

    public bool? IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
