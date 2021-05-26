using System.ComponentModel.DataAnnotations;

namespace WandelApp.Models.Users
{
    public class BaseUserModel
    {
        [Required]
        [StringLength(45)]
        public string Email { get; set; }

        [Required]
        [StringLength(45)]
        public string Password { get; set; }

        [Required]
        [StringLength(45)]
        public string UserName { get; set; }

        [Required]
        public int Age { get; set; }
    }
}
