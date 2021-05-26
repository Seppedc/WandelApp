using System;

namespace WandelApp.Models.UserPets
{
    public class BaseUserPetModel
    {
        public Guid PetId { get; set; }

        public Guid UserId { get; set; }
    }
}
