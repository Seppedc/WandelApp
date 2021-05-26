using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandelApp.api.Repositories;
using WandelApp.api.Games;

namespace WandelApp.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetGameModel>>> GetGames()
        {
            try
            {
                return await _gameRepository.GetGames();
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
        public async Task<ActionResult<GetGameModel>> GetGame(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid gameId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                return await _gameRepository.GetGame(gameId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetGameModel>> PostGame(PostGameModel postGameModel)
        {
            try
            {
                GetGameModel game = await _gameRepository.PostGame(postGameModel);

                return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
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
        public async Task<IActionResult> PutGame(string id, PutGameModel putGameModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid gameId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _gameRepository.PutGame(gameId.ToString(), putGameModel);

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
        public async Task<IActionResult> DeleteGame(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid gameId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _gameRepository.DeleteGame(gameId.ToString());

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
