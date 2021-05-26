using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.Models.Users
{
    public class PutUserModel : BaseUserModel
    {
        [Required]
        public int CurrentPoints { get; set; }

        [Required]
        public int CountPetsCaptured { get; set; }

        public Guid NextLevelId { get; set; }

        public ICollection<Guid> UserPets { get; set; }
        public ICollection<Guid> Friends { get; set; }
        public ICollection<Guid> UserGames { get; set; }
    }
}
