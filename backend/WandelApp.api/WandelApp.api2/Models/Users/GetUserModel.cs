using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Users
{
    public class GetUserModel : BaseUserModel
    {
        public Guid Id { get; set; }

        [Required]
        public int CurrentPoints { get; set; }

        [Required]
        public int CountPetsCaptured { get; set; }

        public Guid NextLevelId { get; set; }
        public string NextLevelName { get; set; }
        public int NextLevelPointsRequired { get; set; }
        public string CurrentLevelName { get; set; }

        public ICollection<Guid> UserPets { get; set; }
        public ICollection<Guid> Friends { get; set; }
        public ICollection<Guid> UserGames { get; set; }
    }
}
