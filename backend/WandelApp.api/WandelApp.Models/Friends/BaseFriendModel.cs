using System;

namespace WandelApp.Models.Friends
{
    public class BaseFriendModel
    {
        public Guid UserId { get; set; }

        public Guid UserFriendId { get; set; }
    }
}
