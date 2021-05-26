using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Games
{
    public class BaseGameModel
    {
        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        [Required]
        public int TotalPointsToEarn { get; set; }
    }
}
