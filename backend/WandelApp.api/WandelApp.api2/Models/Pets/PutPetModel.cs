using System;
using System.Collections.Generic;

namespace WandelApp.api.Pets
{
    public class PutPetModel : BasePetModel
    {
        public ICollection<Guid> UserPets { get; set; }
    }
}
