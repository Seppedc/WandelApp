using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WandelApp.api.Repositories;
using WandelApp.Models.UserPets;

namespace WandelApp.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UserPetController : ControllerBase
    {
        private readonly IUserPetRepository _userPetRepository;

        public UserPetController(IUserPetRepository userPetRepository)
        {
            _userPetRepository = userPetRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetUserPetModel>>> GetUserPets()
        {
            try
            {
                return await _userPetRepository.GetUserPets();
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
        public async Task<ActionResult<GetUserPetModel>> GetUserPet(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userPetId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                return await _userPetRepository.GetUserPet(userPetId.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetUserPetModel>> PostUserPet(PostUserPetModel postUserPetModel)
        {
            try
            {
                GetUserPetModel userPet = await _userPetRepository.PostUserPet(postUserPetModel);

                return CreatedAtAction(nameof(GetUserPet), new { id = userPet.Id }, userPet);
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
        public async Task<IActionResult> PutUserPet(string id, PutUserPetModel putUserPetModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userPetId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _userPetRepository.PutUserPet(userPetId.ToString(), putUserPetModel);

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
        public async Task<IActionResult> DeleteUserPet(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userPetId))
                {
                    throw new Exception("Invalid Guid format.");
                }
                await _userPetRepository.DeleteUserPet(userPetId.ToString());

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
