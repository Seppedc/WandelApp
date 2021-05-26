using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WandelApp.api.RefreshTokens;
using WandelApp.api.Users;

namespace WandelApp.api.Repositories
{
    public interface IUserRepository
    {
        Task<List<GetUserModel>> GetUsers();
        Task<GetUserModel> GetUser(string id);
        Task<GetUserModel> PostUser(PostUserModel postUserModel);
        Task PutUser(string id, PutUserModel putUserModel);
        Task DeleteUser(string id);

        Task<PostAuthenticationResponseModel> Authenticate(PostAuthenticationRequestModel model, string ipAddress);
        Task<PostAuthenticationResponseModel> RefreshToken(string refreshToken, string ipAddress);
        Task<List<GetRefreshTokenModel>> GetUserRefreshTokens(Guid id);
        Task RevokeToken(string token, string ipAddress);
    }
}
