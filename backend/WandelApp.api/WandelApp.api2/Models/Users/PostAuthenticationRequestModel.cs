using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Users
{
    public class PostAuthenticationRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
