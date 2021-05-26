using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Users
{
    public class PostUserModel : BaseUserModel
    {
        [Required]
        [StringLength(45)]
        public string RepeatPassword { get; set; }
    }
}
