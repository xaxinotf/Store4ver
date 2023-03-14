using System;
using System.Collections.Generic;

namespace Store444.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Workaddress { get; set; } = null!;

    public string Homeaddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
