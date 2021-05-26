using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandelApp.api.Repositories;
using WandelApp.Models.Friends;

namespace WandelApp.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class FriendController : ControllerBase
    {
        private readonly IFriendRepository _friendRepository;

        public FriendController(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetFriendModel>>> GetFriends()
        {
            try
            {
                return await _friendRepository.GetFriends();
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
        public async Task<ActionResult<GetFriendModel>> GetFriend(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid friendId))
                {
                    throw new Exception("Invalid Guid format.");
                }

                return await _friendRepository.GetFriend(friendId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetFriendModel>> PostFriend(PostFriendModel postFriendModel)
        {
            try
            {
                GetFriendModel friend = await _friendRepository.PostFriend(postFriendModel);

                return CreatedAtAction(nameof(GetFriend), new { id = friend.Id }, friend);
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
        public async Task<IActionResult> PutFriend(string id, PutFriendModel putFriendModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid friendId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _friendRepository.PutFriend(friendId.ToString(), putFriendModel);

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
        public async Task<IActionResult> DeleteFriend(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid friendId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _friendRepository.DeleteFriend(friendId.ToString());

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
