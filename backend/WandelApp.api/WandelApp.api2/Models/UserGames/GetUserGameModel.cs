using System;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.UserGames
{
    public class GetUserGameModel : BaseUserGameModel
    {
        public Guid Id { get; set; }

        [Required]
        public int CountPlayed { get; set; }

        public string GameName { get; set; }
        public int TotalPointsEarnedWithGame { get; set; }
    }
}
