using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WandelApp.api.Entities;
using WandelApp.api.Helpers;
using WandelApp.api.RefreshTokens;
using WandelApp.api.Users;

namespace WandelApp.api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Context _context;
        private readonly AppSettings _appSettings;

        public UserRepository(Context context, IOptions<AppSettings> appsettings)
        {
            _context = context;
            _appSettings = appsettings.Value;
        }

        public async Task DeleteUser(string id)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user.UserGames.Count() > 0)
            {
                _context.UserGames.RemoveRange(user.UserGames);
            }
            if (user.UserPets.Count() > 0)
            {
                _context.UserPets.RemoveRange(user.UserPets);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<GetUserModel> GetUser(string id)
        {
            GetUserModel user = await _context.Users
                .Include(x => x.UserGames)
                .Include(x => x.UserPets)
                .Select(x => new GetUserModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Password = x.Password,
                    UserName = x.UserName,
                    Age = x.Age,
                    CurrentPoints = x.CurrentPoints,
                    CountPetsCaptured = x.CountPetsCaptured,
                    NextLevelId = x.NextLevelId,
                    NextLevelName = x.NextLevel.Name,
                    NextLevelPointsRequired = x.NextLevel.MinimumPoints,
                    CurrentLevelName = x.CurrentLevelName,
                    UserGames = x.UserGames.Select(x => x.Id).ToList(),
                    UserPets = x.UserPets.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null)
            {
                throw new Exception("No user found");
            }

            return user;
        }

        public async Task<List<GetUserModel>> GetUsers()
        {
            List<GetUserModel> users = await _context.Users
                .Include(x => x.UserGames)
                .Include(x => x.UserPets)
                .Select(x => new GetUserModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Password = x.Password,
                    UserName = x.UserName,
                    Age = x.Age,
                    CurrentPoints = x.CurrentPoints,
                    CountPetsCaptured = x.CountPetsCaptured,
                    NextLevelId = x.NextLevelId,
                    NextLevelName = x.NextLevel.Name,
                    NextLevelPointsRequired = x.NextLevel.MinimumPoints,
                    CurrentLevelName = x.CurrentLevelName,
                    UserGames = x.UserGames.Select(x => x.Id).ToList(),
                    UserPets = x.UserPets.Select(x => x.Id).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return users;
        }

        public async Task<GetUserModel> PostUser(PostUserModel postUserModel)
        {
            EntityEntry<User> result = await _context.Users.AddAsync(new User
            {
                Email = postUserModel.Email,
                Password = postUserModel.Password,
                RepeatPassword = postUserModel.RepeatPassword,
                Age = postUserModel.Age,
                UserName = postUserModel.UserName,
                CurrentPoints = 0,
                CountPetsCaptured = 0,
                CurrentLevelName = _context.Levels.ToList()[0].Name,
                NextLevelId = _context.Levels.ToList()[1].Id
            });

            await _context.SaveChangesAsync();
            return await GetUser(result.Entity.Id.ToString());
        }

        public async Task PutUser(string id, PutUserModel putUserModel)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (user == null)
            {
                throw new Exception("No user found");
            }

            user.Email = putUserModel.Email;
            user.Password = putUserModel.Password;
            user.RepeatPassword = putUserModel.Password;
            user.UserName = putUserModel.UserName;
            user.Age = putUserModel.Age;
            user.CurrentPoints = putUserModel.CurrentPoints;
            user.CountPetsCaptured = putUserModel.CountPetsCaptured;
            user.NextLevelId = putUserModel.NextLevelId;
            await _context.SaveChangesAsync();
        }

        //Authentication
        public async Task<PostAuthenticationResponseModel> Authenticate(PostAuthenticationRequestModel model, string ipAddress)
        {
            var user = await _context.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                throw new Exception("Problem with loggin in. Check provided credentials.");
            }

            // Encrypt passwords
            if (!user.Password.Equals(model.Password))
            {
                throw new Exception("Problem with loggin in. Check provided credentials.");
            }

            string jwtToken = GenerateJwtToken(user);
            RefreshToken refreshToken = GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new PostAuthenticationResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }
        public async Task<PostAuthenticationResponseModel> RefreshToken(string token, string ipAddress)
        {
            User user = await _context.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                throw new Exception("No user found with this token.");
            }

            RefreshToken refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                throw new Exception("Refresh token is no longer active.");
            };

            RefreshToken newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            string jwtToken = GenerateJwtToken(user);
            user.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();
            return new PostAuthenticationResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }
        public async Task RevokeToken(string token, string ipAddress)
        {
            User user = await _context.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                throw new Exception("No user found with this token.");
            }

            RefreshToken refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                throw new Exception("Refresh token is no longer active.");
            };

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            await _context.SaveChangesAsync();
        }

        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("UserName", user.UserName)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "WandelApp API",
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddMinutes(45),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            using RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(3),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<List<GetRefreshTokenModel>> GetUserRefreshTokens(Guid id)
        {
            List<GetRefreshTokenModel> refreshTokens = await _context.RefreshTokens
                .Where(x => x.UserId == id)
                .Select(x => new GetRefreshTokenModel
                {
                    Id = x.Id,
                    Token = x.Token,
                    Expires = x.Expires,
                    IsExpired = x.IsExpired,
                    Created = x.Created,
                    CreatedByIp = x.CreatedByIp,
                    Revoked = x.Revoked,
                    RevokedByIp = x.RevokedByIp,
                    ReplacedByToken = x.ReplacedByToken,
                    IsActive = x.IsActive
                })
                .AsNoTracking()
                .ToListAsync();
            if (refreshTokens.Count == 0)
            {
                throw new Exception("No refresh tokens found.");
            }
            return refreshTokens;
        }
    }
}
