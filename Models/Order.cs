using System;
using System.Collections.Generic;

namespace Store444.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int Status { get; set; }

    public int ShipType { get; set; }

    public int PaymentTypeId { get; set; }

    public int UserId { get; set; }

    public string? DeliveryAddress { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual ShipType ShipTypeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
