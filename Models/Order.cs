using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Store444.Models;

public partial class Order
{
    public int Id { get; set; }
   
    [DisplayName("Статус замовлення")]
    public int Status { get; set; }

    public int? ShipType { get; set; }

    public int? PaymentTypeId { get; set; }

    public string? UserId { get; set; }

    [DisplayName("Ваша адреса")]
    public string? DeliveryAddress { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual PaymentType? PaymentType { get; set; }


    [DisplayName("Номер замовлення")]
    public virtual ShipType? ShipTypeNavigation { get; set; }
}
