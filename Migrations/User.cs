using System;
using System.Collections.Generic;

namespace WebUI.Migrations;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Midname { get; set; }

    public DateOnly CreatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = [];

    public virtual ICollection<News> News { get; set; } = [];
    
}
