using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Infrastructure.Repositories;

namespace Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainGroupController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public MainGroupController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MainGroupDto>>> GetAll()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var mainGroups = await unitOfWork.MainGroupRepository.GetAllAsync();
            return Ok(mainGroups);
        }

        [HttpGet("GetWithId")]
        public async Task<ActionResult<MainGroupDto>> GetById([FromQuery]int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var mainGroup = await unitOfWork.MainGroupRepository.GetByIdAsync(id);
            if (mainGroup == null) return NotFound();

            return Ok(mainGroup);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MainGroupDto mainGroupDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            await unitOfWork.MainGroupRepository.AddAsync(mainGroupDto);
            return CreatedAtAction(nameof(GetById), new { id = mainGroupDto.Id }, mainGroupDto);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromQuery]int id, [FromBody] MainGroupDto mainGroupDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            if (id != mainGroupDto.Id) return BadRequest();

            await unitOfWork.MainGroupRepository.UpdateAsync(mainGroupDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var mainGroup = await unitOfWork.MainGroupRepository.GetByIdAsync(id);
            if (mainGroup == null) return NotFound();

            await unitOfWork.MainGroupRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
