using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.api.UserGames;

namespace WandelApp.api.Repositories
{
    public class UserGameRepository : IUserGameRepository
    {
        private readonly Context _context;

        public UserGameRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteUserGame(string id)
        {
            UserGame userGame = await _context.UserGames
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (userGame == null)
            {
                throw new Exception("No userGame found");
            }

            _context.UserGames.Remove(userGame);
            await _context.SaveChangesAsync();
        }

        public async Task<GetUserGameModel> GetUserGame(string id)
        {
            GetUserGameModel getUserGameModel = await _context.UserGames
                .Select(x => new GetUserGameModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    GameId = x.GameId,
                    GameName = x.Game.Name,
                    TotalPointsEarnedWithGame = x.CountPlayed * x.Game.TotalPointsToEarn,
                    CountPlayed = x.CountPlayed
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (getUserGameModel == null)
            {
                throw new Exception("No userGame found");
            }

            return getUserGameModel;
        }

        public async Task<List<GetUserGameModel>> GetUserGames()
        {
            List<GetUserGameModel> getUserGameModels = await _context.UserGames
                .Select(x => new GetUserGameModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    GameId = x.GameId,
                    CountPlayed = x.CountPlayed
                })
                .AsNoTracking()
                .ToListAsync();

            return getUserGameModels;
        }

        public async Task<GetUserGameModel> PostUserGame(PostUserGameModel postUserGameModel)
        {
            EntityEntry<UserGame> result = await _context.UserGames.AddAsync(new UserGame
            {
                UserId = postUserGameModel.UserId,
                GameId = postUserGameModel.GameId,
                CountPlayed = 0
            });

            await _context.SaveChangesAsync();
            return await GetUserGame(result.Entity.Id.ToString());
        }

        public async Task PutUserGame(string id, PutUserGameModel putUserGameModel)
        {
            UserGame userGame = await _context.UserGames
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (userGame == null)
            {
                throw new Exception("No userGame found");
            }

            userGame.CountPlayed += 1;
            await _context.SaveChangesAsync();
        }
    }
}
