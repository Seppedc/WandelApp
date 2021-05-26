using System;
using System.Collections.Generic;

namespace WandelApp.api.Levels
{
    public class GetLevelModel : BaseLevelModel
    {
        public Guid Id { get; set; }

        public ICollection<Guid> Users { get; set; }
    }
}
