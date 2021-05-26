using System;
using System.ComponentModel.DataAnnotations;

namespace WandelApp.api.Entities
{
    public class Friend
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid UserFriendId { get; set; }
        public User UserFriend { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
    }
}
