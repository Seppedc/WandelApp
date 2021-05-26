using System;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Entities
{
    public class UserGame
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid GameId { get; set; }
        public Game Game { get; set; }

        [Required]
        public int CountPlayed { get; set; }
    }
}
