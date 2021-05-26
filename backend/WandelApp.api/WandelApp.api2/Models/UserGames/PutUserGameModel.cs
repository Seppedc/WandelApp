using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.UserGames
{
    public class PutUserGameModel : BaseUserGameModel
    {
        [Required]
        public int CountPlayed { get; set; }
    }
}
