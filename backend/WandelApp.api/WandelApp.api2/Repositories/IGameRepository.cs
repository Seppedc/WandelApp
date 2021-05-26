using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.api.Games;

namespace WandelApp.api.Repositories
{
    public interface IGameRepository
    {
        Task<List<GetGameModel>> GetGames();
        Task<GetGameModel> GetGame(string id);
        Task<GetGameModel> PostGame(PostGameModel postGameModel);
        Task PutGame(string id, PutGameModel putGameModel);
        Task DeleteGame(string id);
    }
}
