using System;

namespace WandelApp.api.Friends
{
    public class BaseFriendModel
    {
        public Guid UserId { get; set; }

        public Guid UserFriendId { get; set; }
    }
}
