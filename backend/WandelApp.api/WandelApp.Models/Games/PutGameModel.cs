using System;
using System.Collections.Generic;

namespace WandelApp.Models.Games
{
    public class PutGameModel : BaseGameModel
    {
        public ICollection<Guid> UserGames { get; set; }
    }
}
