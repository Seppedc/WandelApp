using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.Models.Games;

namespace WandelApp.api.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly Context _context;

        public GameRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteGame(string id)
        {
            Game game = await _context.Games
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (game.UserGames.Count > 0)
            {
                _context.UserGames.RemoveRange(game.UserGames);
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<GetGameModel> GetGame(string id)
        {
            GetGameModel getGameModel = await _context.Games
                .Select(x => new GetGameModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TotalPointsToEarn = x.TotalPointsToEarn,
                    UserGames = x.UserGames.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return getGameModel;
        }

        public async Task<string> GetGameIdByName(string gameName)
        {
            Game game = await _context.Games
                .FirstOrDefaultAsync(x => x.Name == gameName);
            return game.Id.ToString();
        }

        public async Task<List<GetGameModel>> GetGames()
        {
            List<GetGameModel> getGameModels = await _context.Games
                .Select(x => new GetGameModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TotalPointsToEarn = x.TotalPointsToEarn,
                    UserGames = x.UserGames.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return getGameModels;
        }

        public async Task<GetGameModel> PostGame(PostGameModel postGameModel)
        {
            EntityEntry<Game> result = await _context.Games.AddAsync(new Game
            {
                Name = postGameModel.Name,
                TotalPointsToEarn = postGameModel.TotalPointsToEarn
            });

            await _context.SaveChangesAsync();
            return await GetGame(result.Entity.Id.ToString());
        }

        public async Task PutGame(string id, PutGameModel putGameModel)
        {
            Game game = await _context.Games
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            game.Name = putGameModel.Name;
            game.TotalPointsToEarn = putGameModel.TotalPointsToEarn;
            await _context.SaveChangesAsync();
        }
    }
}
