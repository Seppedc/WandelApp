using System;
using System.Text.Json.Serialization;

namespace WandelApp.api.Users
{
    public class PostAuthenticationResponseModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public string JwtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
