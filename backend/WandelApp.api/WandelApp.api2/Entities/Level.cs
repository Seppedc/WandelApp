using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Entities
{
    public class Level
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        [Required]
        public int MinimumPoints { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
