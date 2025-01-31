    using System;
    using System.Collections.Generic;

    namespace WebUI.Migrations;

    public partial class News
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public int AuthorId { get; set; }

        public int ViewsCount { get; set; }

        public byte[]? CoverImage { get; set; }

        public virtual User Author { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
