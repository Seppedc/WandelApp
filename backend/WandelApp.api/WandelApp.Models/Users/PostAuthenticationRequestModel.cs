using System.ComponentModel.DataAnnotations;

namespace WandelApp.Models.Users
{
    public class PostAuthenticationRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
