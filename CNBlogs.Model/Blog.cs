using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CNBlogs.Model
{
    public class Blog
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(32)]
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual User User { get; set; }
    }
}
