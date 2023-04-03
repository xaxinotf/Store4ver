using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

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

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
