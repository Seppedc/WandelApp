using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.api.UserGames;

namespace WandelApp.api.Repositories
{
    public interface IUserGameRepository
    {
        Task<List<GetUserGameModel>> GetUserGames();
        Task<GetUserGameModel> GetUserGame(string id);
        Task<GetUserGameModel> PostUserGame(PostUserGameModel postUserGameModel);
        Task PutUserGame(string id, PutUserGameModel putUserGameModel);
        Task DeleteUserGame(string id);
    }
}
