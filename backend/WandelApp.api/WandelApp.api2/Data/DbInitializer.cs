using Microsoft.Extensions.DependencyInjection;
using System;
using WandelApp.api.Entities;

namespace WandelApp.api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            Context _context = serviceProvider.GetRequiredService<Context>();

            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();

            Level startLevel = new Level
            {
                Name = "Beginner",
                MinimumPoints = 0
            };
            _context.Levels.Add(startLevel);
            Level firstLevel = new Level
            {
                Name = "First Level",
                MinimumPoints = 50
            };
            _context.Levels.Add(firstLevel);

            User bob = new User
            {
                Email = "bob@gmail.com",
                Password = "Test1234",
                RepeatPassword = "Test1234",
                UserName = "Bob",
                Age = 21,
                CurrentPoints = 0,
                CountPetsCaptured = 0,
                NextLevelId = firstLevel.Id,
                CurrentLevelName = startLevel.Name
            };
            _context.Users.Add(bob);

            Game simonSays = new Game
            {
                Name = "Simon Says",
                TotalPointsToEarn = 50
            };
            _context.Games.Add(simonSays);

            UserGame bobSimonSays = new UserGame
            {
                UserId = bob.Id,
                GameId = simonSays.Id,
                CountPlayed = 5
            };
            _context.UserGames.Add(bobSimonSays);

            _context.SaveChanges();
        }
    }
}
