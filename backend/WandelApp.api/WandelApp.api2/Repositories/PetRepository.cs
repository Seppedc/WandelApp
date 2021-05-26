using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.api.Pets;

namespace WandelApp.api.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly Context _context;

        public PetRepository(Context context)
        {
            _context = context;
        }

        public async Task DeletePet(string id)
        {
            Pet pet = await _context.Pets
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (pet.UserPets.Count() > 0)
            {
                _context.UserPets.RemoveRange(pet.UserPets);
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }

        public async Task<GetPetModel> GetPet(string id)
        {
            GetPetModel getPetModel = await _context.Pets
                .Select(x => new GetPetModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Experience = x.Experience,
                    Level = x.Level,
                    UserPets = x.UserPets.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (getPetModel == null)
            {
                throw new Exception("No pet found");
            }

            return getPetModel;
        }

        public async Task<List<GetPetModel>> GetPets()
        {
            List<GetPetModel> getPetModels = await _context.Pets
                 .Select(x => new GetPetModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Experience = x.Experience,
                     Level = x.Level,
                     UserPets = x.UserPets.Select(x => x.Id).ToList()
                 })
                 .AsNoTracking()
                 .ToListAsync();

            return getPetModels;
        }

        public async Task<GetPetModel> PostPet(PostPetModel postPetModel)
        {
            EntityEntry<Pet> result = await _context.Pets.AddAsync(new Pet
            {
                Name = postPetModel.Name,
                Experience = postPetModel.Experience,
                Level = postPetModel.Level
            });

            await _context.SaveChangesAsync();
            return await GetPet(result.Entity.Id.ToString());
        }

        public async Task PutPet(string id, PutPetModel putPetModel)
        {
            Pet pet = await _context.Pets
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (pet == null)
            {
                throw new Exception("No pet found");
            }

            pet.Name = putPetModel.Name;
            pet.Experience = putPetModel.Experience;
            pet.Level = putPetModel.Level;
            await _context.SaveChangesAsync();
        }
    }
}
