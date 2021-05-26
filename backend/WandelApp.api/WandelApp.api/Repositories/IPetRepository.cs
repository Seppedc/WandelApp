using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.Models.Pets;

namespace WandelApp.api.Repositories
{
    public interface IPetRepository
    {
        Task<List<GetPetModel>> GetPets();
        Task<GetPetModel> GetPet(string id);
        Task<GetPetModel> PostPet(PostPetModel postPetModel);
        Task PutPet(string id, PutPetModel putPetModel);
        Task DeletePet(string id);
    }
}
