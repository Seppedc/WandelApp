using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandelApp.api.Repositories;
using WandelApp.api.Levels;

namespace WandelApp.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class LevelController : ControllerBase
    {
        private readonly ILevelRepository _levelRepository;

        public LevelController(ILevelRepository levelRepository)
        {
            _levelRepository = levelRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetLevelModel>>> GetLevels()
        {
            try
            {
                return await _levelRepository.GetLevels();
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
        public async Task<ActionResult<GetLevelModel>> GetLevel(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid levelId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                return await _levelRepository.GetLevel(levelId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetLevelModel>> PostLevel(PostLevelModel postLevelModel)
        {
            try
            {
                GetLevelModel level = await _levelRepository.PostLevel(postLevelModel);

                return CreatedAtAction(nameof(GetLevel), new { id = level.Id }, level);
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
        public async Task<IActionResult> PutLevel(string id, PutLevelModel putLevelModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid levelId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _levelRepository.PutLevel(levelId.ToString(), putLevelModel);

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
        public async Task<IActionResult> DeleteLevel(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid levelId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _levelRepository.DeleteLevel(levelId.ToString());

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
