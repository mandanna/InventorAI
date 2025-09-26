using System;
using System.Collections.Generic;

namespace InventorAi_api.EntityModels;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int LoyaltyPoints { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
