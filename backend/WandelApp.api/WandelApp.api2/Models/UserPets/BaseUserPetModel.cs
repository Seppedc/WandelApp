using System;

namespace WandelApp.api.UserPets
{
    public class BaseUserPetModel
    {
        public Guid PetId { get; set; }

        public Guid UserId { get; set; }
    }
}
