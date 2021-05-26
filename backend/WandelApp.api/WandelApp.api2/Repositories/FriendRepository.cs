using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.api.Friends;

namespace WandelApp.api.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly Context _context;

        public FriendRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteFriend(string id)
        {
            Friend friend = await _context.Friends
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (friend == null)
            {
                throw new Exception("Not friend found");
            }

            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }

        public async Task<GetFriendModel> GetFriend(string id)
        {
            GetFriendModel getFriendModel = await _context.Friends
                .Select(x => new GetFriendModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserFriendId = x.UserFriendId,
                    DateCreated = x.DateCreated
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (getFriendModel == null)
            {
                throw new Exception("No friend found");
            }

            return getFriendModel;
        }

        public async Task<List<GetFriendModel>> GetFriends()
        {
            List<GetFriendModel> getFriendModels = await _context.Friends
                .Select(x => new GetFriendModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserFriendId = x.UserFriendId,
                    DateCreated = x.DateCreated
                })
                .AsNoTracking()
                .ToListAsync();

            return getFriendModels;
        }

        public async Task<GetFriendModel> PostFriend(PostFriendModel postFriendModel)
        {
            EntityEntry<Friend> result = await _context.Friends.AddAsync(new Friend
            {
                UserId = postFriendModel.UserId,
                UserFriendId = postFriendModel.UserFriendId,
                DateCreated = DateTime.Today
            });

            await _context.SaveChangesAsync();
            return await GetFriend(result.Entity.Id.ToString());
        }

        public async Task PutFriend(string id, PutFriendModel putFriendModel)
        {
            Friend friend = await _context.Friends
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (friend == null)
            {
                throw new Exception("No friend found");
            }

            friend.UserId = putFriendModel.UserId;
            friend.UserFriendId = putFriendModel.UserFriendId;
            await _context.SaveChangesAsync();
        }
    }
}
