using System;

namespace WandelApp.Models.UserGames
{
    public class BaseUserGameModel
    {
        public Guid UserId { get; set; }

        public Guid GameId { get; set; }
    }
}
