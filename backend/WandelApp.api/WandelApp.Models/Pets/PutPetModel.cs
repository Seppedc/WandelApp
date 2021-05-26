using System;
using System.Collections.Generic;

namespace WandelApp.Models.Pets
{
    public class PutPetModel : BasePetModel
    {
        public ICollection<Guid> UserPets { get; set; }
    }
}
