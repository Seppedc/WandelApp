using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Email { get; set; }

        [Required]
        [StringLength(45)]
        public string Password { get; set; }

        [Required]
        [StringLength(45)]
        public string RepeatPassword { get; set; }

        [Required]
        [StringLength(45)]
        public string UserName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public int CurrentPoints { get; set; }

        [Required]
        public int CountPetsCaptured { get; set; }

        public Guid NextLevelId { get; set; }
        public Level NextLevel { get; set; }
        public string CurrentLevelName { get; set; }

        public ICollection<UserPet> UserPets { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<UserGame> UserGames { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
