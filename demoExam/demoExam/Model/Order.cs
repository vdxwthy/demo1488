using System;
using System.Collections.Generic;

namespace demoExam.Model;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly DateOrder { get; set; }

    public DateOnly DateDelivery { get; set; }

    public int PickupPointId { get; set; }

    public int UserId { get; set; }

    public int Code { get; set; }

    public string StatusOrder { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PickupPoint PickupPoint { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
