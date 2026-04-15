using System;
using System.Collections.Generic;

namespace demoExam.Model;

public partial class Storage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public string Unit { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
