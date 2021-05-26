using Microsoft.EntityFrameworkCore;
using System;

namespace WandelApp.api.Entities
{
    public class Context : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<UserPet> UserPets { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            if (builder == null) { throw new ArgumentNullException(nameof(builder)); }

            builder.Entity<User>(user =>
            {

                user.HasOne(x => x.NextLevel)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.NextLevelId)
                .IsRequired();
            });
            builder.Entity<Friend>(friend =>
            {
                friend.HasOne(x => x.User)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<UserGame>(userGame =>
            {
                userGame.HasOne(x => x.User)
                .WithMany(x => x.UserGames)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

                userGame.HasOne(x => x.Game)
                .WithMany(x => x.UserGames)
                .HasForeignKey(x => x.GameId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<UserPet>(userPet =>
            {
                userPet.HasOne(x => x.User)
                .WithMany(x => x.UserPets)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

                userPet.HasOne(x => x.Pet)
                .WithMany(x => x.UserPets)
                .HasForeignKey(x => x.PetId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<RefreshToken>(rt =>
            {
                rt.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
            });
        }
    }
}
