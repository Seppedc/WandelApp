using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Entities
{
    public class Game
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        [Required]
        public int TotalPointsToEarn { get; set; }

        public ICollection<UserGame> UserGames { get; set; }
    }
}
