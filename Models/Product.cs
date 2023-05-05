using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Store444.Models;

public partial class Product
{
    public int Id { get; set; }
    [DisplayName("Ім'я товару")]
    public string Name { get; set; } = null!;
    [DisplayName("Форма випукску та дозування")]
    public string RelaiseFromAndDosing { get; set; } = null!;
    [DisplayName("Термін придатності")]
    public string ShelfLife { get; set; } = null!;
    [DisplayName("Ціна")]
    public double Price { get; set; }
    public string? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
