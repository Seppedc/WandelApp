using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandelApp.api.Repositories;
using WandelApp.models.RefreshTokens;
using WandelApp.Models.Users;

namespace WandelApp.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetUserModel>>> GetUsers()
        {
            try
            {
                return await _userRepository.GetUsers();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserModel>> GetUser(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new Exception("Invalid Guid format.");
                }

                return await _userRepository.GetUser(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostAuthenticationResponseModel>> PostUser(PostUserModel postUserModel)
        {
            try
            {
                GetUserModel user = await _userRepository.PostUser(postUserModel);

                PostAuthenticationRequestModel authenticationRequestModel = new PostAuthenticationRequestModel
                {
                    Email = user.Email,
                    Password = user.Password
                };
                return await Authenticate(authenticationRequestModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUser(string id, PutUserModel putUserModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new Exception("Invalid Guid format.");
                }

                await _userRepository.PutUser(id, putUserModel);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new Exception("Invalid Guid format.");
                }

                await _userRepository.DeleteUser(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Authentication
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostAuthenticationResponseModel>> Authenticate(PostAuthenticationRequestModel postAuthenticateRequestModel)
        {
            try
            {
                PostAuthenticationResponseModel postAuthenticateResponseModel = await _userRepository.Authenticate(postAuthenticateRequestModel, IpAddress());
                SetTokenCookie(postAuthenticateResponseModel.RefreshToken);
                return postAuthenticateResponseModel;
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostAuthenticationResponseModel>> RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["WandelApp.RefreshToken"];
                PostAuthenticationResponseModel postAuthenticateResponseModel = await _userRepository.RefreshToken(refreshToken, IpAddress());
                SetTokenCookie(postAuthenticateResponseModel.RefreshToken);
                return postAuthenticateResponseModel;
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet("{id}/refresh-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<GetRefreshTokenModel>>> GetUserRefreshTokens(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new Exception("Invalid Guid format.");
                }

                List<GetRefreshTokenModel> refreshTokens = await _userRepository.GetUserRefreshTokens(userId);
                return refreshTokens;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("revoke-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RevokeToken(PostRevokeTokenRequestModel postRevokeTokenRequestModel)
        {
            try
            {
                string token = postRevokeTokenRequestModel.Token ?? Request.Cookies["WandelApp.RefreshToken"];
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Refresh token is required.");
                }
                await _userRepository.RevokeToken(token, IpAddress());
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // Helper methods
        private void SetTokenCookie(string token)
        {
            CookieOptions cookie = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(3),
                IsEssential = true
            };
            Response.Cookies.Append("WandelApp.RefreshToken", token, cookie);
        }
        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
