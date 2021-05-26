using System.ComponentModel.DataAnnotations;

namespace WandelApp.Models.UserGames
{
    public class PutUserGameModel : BaseUserGameModel
    {
        [Required]
        public int CountPlayed { get; set; }
    }
}
