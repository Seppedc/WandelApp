using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Entities
{
    public class Pet
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Experience { get; set; }

        [Required]
        public int Level { get; set; }

        public ICollection<UserPet> UserPets { get; set; }
    }
}
