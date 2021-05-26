using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandelApp.api.Repositories;
using WandelApp.Models.UserGames;

namespace WandelApp.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UserGameController : ControllerBase
    {
        private readonly IUserGameRepository _userGameRepository;

        public UserGameController(IUserGameRepository userGameRepository)
        {
            _userGameRepository = userGameRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetUserGameModel>>> GetUserGames()
        {
            try
            {
                return await _userGameRepository.GetUserGames();
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
        public async Task<ActionResult<GetUserGameModel>> GetUserGame(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userGameId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                return await _userGameRepository.GetUserGame(userGameId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserGameModel>> PostUserGame(PostUserGameModel postUserGameModel)
        {
            try
            {
                GetUserGameModel userGame = await _userGameRepository.PostUserGame(postUserGameModel);

                return CreatedAtAction(nameof(GetUserGame), new { id = userGame.Id }, userGame);
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
        public async Task<IActionResult> PutUserGame(string id, PutUserGameModel putUserGameModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userGameId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _userGameRepository.PutUserGame(userGameId.ToString(), putUserGameModel);

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
        public async Task<IActionResult> DeleteUserGame(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userGameId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _userGameRepository.DeleteUserGame(userGameId.ToString());

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
