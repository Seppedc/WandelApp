using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Pets
{
    public class BasePetModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Experience { get; set; }

        [Required]
        public int Level { get; set; }
    }
}
