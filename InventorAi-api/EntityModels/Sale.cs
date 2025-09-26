using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Sale
{
    public int SaleId { get; set; }

    public int UserId { get; set; }

    public int? CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal NetAmount { get; set; }

    public string Currency { get; set; } = null!;

    public DateTime SaleDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int StatusId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
