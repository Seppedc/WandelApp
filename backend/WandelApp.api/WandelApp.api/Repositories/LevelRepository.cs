using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.Models.Levels;

namespace WandelApp.api.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly Context _context;

        public LevelRepository(Context context)
        {
            _context = context;
        }

        public async Task DeleteLevel(string id)
        {
            Level level = await _context.Levels
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();
        }

        public async Task<GetLevelModel> GetLevel(string id)
        {
            GetLevelModel getLevelModel = await _context.Levels
                .Select(x => new GetLevelModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MinimumPoints = x.MinimumPoints,
                    Users = x.Users.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            return getLevelModel;
        }

        public async Task<List<GetLevelModel>> GetLevels()
        {
            List<GetLevelModel> getLevelModels = await _context.Levels
                .Select(x => new GetLevelModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MinimumPoints = x.MinimumPoints,
                    Users = x.Users.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return getLevelModels;
        }

        public async Task<GetLevelModel> PostLevel(PostLevelModel postLevelModel)
        {
            EntityEntry<Level> result = await _context.Levels.AddAsync(new Level
            {
                Name = postLevelModel.Name,
                MinimumPoints = postLevelModel.MinimumPoints
            });

            await _context.SaveChangesAsync();
            return await GetLevel(result.Entity.Id.ToString());
        }

        public async Task PutLevel(string id, PutLevelModel putLevelModel)
        {
            Level level = await _context.Levels
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            level.Name = putLevelModel.Name;
            level.MinimumPoints = putLevelModel.MinimumPoints;
            await _context.SaveChangesAsync();
        }
    }
}
