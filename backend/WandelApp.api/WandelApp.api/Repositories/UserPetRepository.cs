using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.Models.UserPets;

namespace WandelApp.api.Repositories
{
    public class UserPetRepository : IUserPetRepository
    {
        private readonly Context _context;

        public UserPetRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteUserPet(string id)
        {
            UserPet userPet = await _context.UserPets
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            _context.UserPets.Remove(userPet);
            await _context.SaveChangesAsync();
        }

        public async Task<GetUserPetModel> GetUserPet(string id)
        {
            GetUserPetModel getUserPetModel = await _context.UserPets
                .Select(x => new GetUserPetModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    PetId = x.PetId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return getUserPetModel;
        }

        public async Task<List<GetUserPetModel>> GetUserPets()
        {
            List<GetUserPetModel> getUserPetModels = await _context.UserPets
                .Select(x => new GetUserPetModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    PetId = x.PetId
                })
                .AsNoTracking()
                .ToListAsync();

            return getUserPetModels;
        }

        public async Task<GetUserPetModel> PostUserPet(PostUserPetModel postUserPetModel)
        {
            EntityEntry<UserPet> result = await _context.UserPets.AddAsync(new UserPet
            {
                UserId = postUserPetModel.UserId,
                PetId = postUserPetModel.PetId
            });

            await _context.SaveChangesAsync();
            return await GetUserPet(result.Entity.Id.ToString());
        }

        public async Task PutUserPet(string id, PutUserPetModel putUserPetModel)
        {
            UserPet userPet = await _context.UserPets
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            userPet.UserId = putUserPetModel.UserId;
            userPet.PetId = putUserPetModel.PetId;
            await _context.SaveChangesAsync();
        }
    }
}
