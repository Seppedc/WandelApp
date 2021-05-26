using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.Models.UserPets;

namespace WandelApp.api.Repositories
{
    public interface IUserPetRepository
    {
        Task<List<GetUserPetModel>> GetUserPets();
        Task<GetUserPetModel> GetUserPet(string id);
        Task<GetUserPetModel> PostUserPet(PostUserPetModel postUserPetModel);
        Task PutUserPet(string id, PutUserPetModel putUserPetModel);
        Task DeleteUserPet(string id);
    }
}
