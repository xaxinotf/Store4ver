using System;
using System.Collections.Generic;

namespace Store444.Models;

public partial class ShipType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
