using System.ComponentModel.DataAnnotations;

namespace WandelApp.Models.Levels
{
    public class BaseLevelModel
    {
        [Required]
        [StringLength(45)]
        public string Name { get; set; }

        [Required]
        public int MinimumPoints { get; set; }
    }
}
