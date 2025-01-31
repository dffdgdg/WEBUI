using System;
using System.Collections.Generic;

namespace WebUI.Migrations;

public partial class Comment
{
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int NewsId { get; set; }

    public int UserId { get; set; }

    public virtual News News { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
