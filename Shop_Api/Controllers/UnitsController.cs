using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_Core.DTOS;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using Shop_Infrastructure.Repositories;

namespace Shop_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()

        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var units = unitOfWork.UnitRepository.GetAll();
            return Ok(units);
        }

        [HttpGet("GetWithId")]
        public IActionResult GetById([FromQuery] int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var unit = unitOfWork.UnitRepository.GetById(id);
            if (unit == null)
            {
                return NotFound();
            }
            return Ok(unit);
        }


        [HttpPost]
        public IActionResult Add([FromBody] UD unit)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            if (unit == null || string.IsNullOrEmpty(unit.Name))
            {
                return BadRequest("Invalid unit data.");
            }

            unitOfWork.UnitRepository.Add(unit);

            return CreatedAtAction(nameof(GetById), new { id = unit.Id }, new
            {
                Message = "The unit has been added successfully.",
                ID = unit.Id,
                Name = unit.Name
            });
        }

        [HttpPut]
        
        public IActionResult Update([FromQuery] int id, [FromBody] UD unit)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var existingUnit = unitOfWork.UnitRepository.GetById(id);

            if (existingUnit == null)
            {
                return NotFound();  
            }

            existingUnit.Name = unit.Name;

            unitOfWork.UnitRepository.Update(existingUnit);

            var response = new
            {
                Message = "The unit has been updated successfully.",
                ID = existingUnit.Id,
                Name = existingUnit.Name
            };

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery]int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var existingUnit = unitOfWork.UnitRepository.GetById(id);
            if (existingUnit == null)
            {
                return NotFound();
            }

            unitOfWork.UnitRepository.Delete(id);
            return NoContent();
        }
    }
}
