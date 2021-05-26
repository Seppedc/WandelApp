using System;
using System.Collections.Generic;

namespace WandelApp.api.Pets
{
    public class GetPetModel : BasePetModel
    {
        public Guid Id { get; set; }

        public ICollection<Guid> UserPets { get; set; }
    }
}
