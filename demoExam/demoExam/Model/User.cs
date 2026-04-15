using System;
using System.Collections.Generic;

namespace demoExam.Model;

public partial class User
{
    public int Id { get; set; }

    public string Lastname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Midname { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
