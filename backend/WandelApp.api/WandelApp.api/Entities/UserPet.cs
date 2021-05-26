using System;

namespace WandelApp.api.Entities
{
    public class UserPet
    {
        public Guid Id { get; set; }

        public Guid PetId { get; set; }
        public Pet Pet { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
