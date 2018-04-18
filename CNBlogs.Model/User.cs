using System;
using System.ComponentModel.DataAnnotations;

namespace CNBlogs.Model
{
    public class User
    {
        public int UserId { get; set; }
        
        [MaxLength(32),Required]
        public string LoginName { get; set; }

        [MaxLength(32), Required]
        public string Password { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
    }
}
