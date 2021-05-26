using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.Models.UserGames;

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
            UserGame userGame = await _context.UserGames
                .Include(x => x.Game)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == postUserGameModel.UserId && x.GameId == postUserGameModel.GameId);

            string userGameId = "";
            if (userGame == null)
            {
                EntityEntry<UserGame> result = await _context.UserGames.AddAsync(new UserGame
                {
                    UserId = postUserGameModel.UserId,
                    GameId = postUserGameModel.GameId,
                    CountPlayed = 0
                });
                userGameId = result.Entity.Id.ToString();
            } else
            {
                userGame.CountPlayed += 1;
                userGame.User = CheckUserStats(userGame.User, userGame.Game.TotalPointsToEarn);
                userGameId = userGame.Id.ToString();
            }

            await _context.SaveChangesAsync();
            return await GetUserGame(userGameId);
        }
        private User CheckUserStats(User user, int pointsToEarn)
        {
            user.CurrentPoints += pointsToEarn;
            var levels = _context.Levels.ToList();
            for (int i = 0; i < levels.Count(); i++)
            {
                if (user.CurrentPoints <= levels[i].MinimumPoints)
                {
                    user.CurrentLevelName = levels[i].Name;
                    if (i != levels.Count())
                    {
                        user.NextLevelId = levels[i + 1].Id;
                    }
                    return user;
                }
            }
            return null;
        }

        public async Task PutUserGame(string id, PutUserGameModel putUserGameModel)
        {
            UserGame userGame = await _context.UserGames
                .Include(x => x.Game)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            userGame.CountPlayed += 1;
            userGame.User.CurrentPoints += userGame.Game.TotalPointsToEarn;
            await _context.SaveChangesAsync();
        }
    }
}
