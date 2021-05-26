using System;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Friends
{
    public class GetFriendModel : BaseFriendModel
    {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}
