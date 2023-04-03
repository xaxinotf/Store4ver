using System;
using System.Collections.Generic;

namespace Store444.Models;

public partial class OrderProduct
{
    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int Count { get; set; }

    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
