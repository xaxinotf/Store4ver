using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store444.Models;

public partial class Order
{
    public int Id { get; set; }
    [DisplayName("Статус замовлення")]
    public Status Status { get; set; }
    public int? ShipTypeId { get; set; }

    public int? PaymentTypeId { get; set; }

    public string? UserId { get; set; }
    [DisplayName("Адреса доставки")]
    [Required(ErrorMessage = "Введіть адресу доставки!")]
    public string DeliveryAddress { get; set; }

    public int? Count { get; set; }

    public double? Price { get; set; }

    [DisplayName("Тип оплати")]
    public virtual PaymentType? PaymentType { get; set; }

    [DisplayName("Тип доставки")]
    public virtual ShipType? ShipTypeNavigation { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
