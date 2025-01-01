using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_Api.HF;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Infrastructure.Repositories;

namespace Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubGroupController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public SubGroupController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubGroupDto>>> GetAll()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("invalid user token");
            }
            var subGroups = await unitOfWork.SubGroupRepository.GetAllAsync();
            return Ok(subGroups);
        }

        [HttpGet("withid")]
        public async Task<ActionResult<SubGroupDto>> GetById([FromQuery]int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("invalid user token");
            }
            var subGroup = await unitOfWork.SubGroupRepository.GetByIdAsync(id);
            if (subGroup == null) return NotFound();

            return Ok(subGroup);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SubGroupDto subGroupDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            try
            {
                var userId = ExtractClaims.EtractUserId(token);
                if (!userId.HasValue)
                {
                    return Unauthorized("invalid user token");
                }
                await unitOfWork.SubGroupRepository.AddAsync(subGroupDto);
                return CreatedAtAction(nameof(GetById), new { id = subGroupDto.Id }, subGroupDto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromQuery]int id, [FromBody] SubGroupDto subGroupDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("invalid user token");
            }
            if (id != subGroupDto.Id) return BadRequest();

            try
            {
                await unitOfWork.SubGroupRepository.UpdateAsync(subGroupDto);
                return Ok($"Successfully updated SubGroup with Name: {subGroupDto.Name}");
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing");
            }
            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("invalid user token");
            }
            try
            {
                var subGroup = await unitOfWork.SubGroupRepository.GetByIdAsync(id); 
                if (subGroup == null)
                    return NotFound($"SubGroup with ID: {id} does not exist."); 

                await unitOfWork.SubGroupRepository.DeleteAsync(id); 
                return Ok($"Successfully removed SubGroup with ID: {id}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}"); 
            }
        }

    }
}
