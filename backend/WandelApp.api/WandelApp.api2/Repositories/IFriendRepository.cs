using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.api.Friends;

namespace WandelApp.api.Repositories
{
    public interface IFriendRepository
    {
        Task<List<GetFriendModel>> GetFriends();
        Task<GetFriendModel> GetFriend(string id);
        Task<GetFriendModel> PostFriend(PostFriendModel postFriendModel);
        Task PutFriend(string id, PutFriendModel putFriendModel);
        Task DeleteFriend(string id);
    }
}
