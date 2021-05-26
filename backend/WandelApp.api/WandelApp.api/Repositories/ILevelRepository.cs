using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.Models.Levels;

namespace WandelApp.api.Repositories
{
    public interface ILevelRepository
    {
        Task<List<GetLevelModel>> GetLevels();
        Task<GetLevelModel> GetLevel(string id);
        Task<GetLevelModel> PostLevel(PostLevelModel postLevelModel);
        Task PutLevel(string id, PutLevelModel putLevelModel);
        Task DeleteLevel(string id);
    }
}
