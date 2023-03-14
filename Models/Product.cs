using System;
using System.Collections.Generic;

namespace Store444.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string RelaiseFromAndDosing { get; set; } = null!;

    public int Amount { get; set; }

    public string ShelfLife { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
