using System;
using System.Collections.Generic;

namespace WandelApp.Models.Games
{
    public class GetGameModel : BaseGameModel
    {
        public Guid Id { get; set; }

        public ICollection<Guid> UserGames { get; set; }
    }
}
