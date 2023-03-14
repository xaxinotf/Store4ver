using System;
using System.Collections.Generic;

namespace Store444.Models;

public partial class OrderProduct
{
    public int NameProductId { get; set; }

    public int OrdersOrderId { get; set; }

    public int Count { get; set; }

    public decimal Price { get; set; }

    public virtual Product NameProduct { get; set; } = null!;

    public virtual Order OrdersOrder { get; set; } = null!;
}
