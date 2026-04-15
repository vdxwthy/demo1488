using System;
using System.Collections.Generic;

namespace demoExam.Model;

public partial class PickupPoint
{
    public int Id { get; set; }

    public string AddressIndex { get; set; } = null!;

    public string AddressCity { get; set; } = null!;

    public string AddressStreet { get; set; } = null!;

    public string AddressNumberHouse { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
