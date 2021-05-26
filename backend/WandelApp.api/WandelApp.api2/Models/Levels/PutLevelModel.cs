using System;
using System.Collections.Generic;

namespace WandelApp.api.Levels
{
    public class PutLevelModel : BaseLevelModel
    {
        public ICollection<Guid> Users { get; set; }
    }
}
