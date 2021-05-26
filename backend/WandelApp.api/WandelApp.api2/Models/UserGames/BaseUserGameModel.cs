using System;

namespace WandelApp.api.UserGames
{
    public class BaseUserGameModel
    {
        public Guid UserId { get; set; }

        public Guid GameId { get; set; }
    }
}
