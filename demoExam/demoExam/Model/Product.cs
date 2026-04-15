using System;
using System.Collections.Generic;

namespace demoExam.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public int SupplierId { get; set; }

    public int ManufactureId { get; set; }

    public int CategoryId { get; set; }

    public int Discount { get; set; }

    public string? Description { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Manufacturer Manufacture { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Storage> Storages { get; set; } = new List<Storage>();

    public virtual Supplier Supplier { get; set; } = null!;
}
