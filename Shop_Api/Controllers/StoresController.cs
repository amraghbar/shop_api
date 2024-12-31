using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop_Core.DTOS.Store;
using Shop_Core.Interfaces;
using Shop_Infrastructure.Repositories;

namespace Shop_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public StoresController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("GetAll")]
        
        public async Task<ActionResult<IEnumerable<StoreReadDto>>> GetStores()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }

            var stores = await unitOfWork.StoreRepository.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpGet("GetWithId")]
        public async Task<ActionResult<StoreReadDto>> GetStore([FromQuery]int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var store = await unitOfWork.StoreRepository.GetStoreByIdAsync(id);

            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpPost]
        public async Task<ActionResult> AddStore(StoreDto storeDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            await unitOfWork.StoreRepository.AddStoreAsync(storeDto);
            return CreatedAtAction(nameof(GetStore), new { id = storeDto.Name }, storeDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateStore([FromQuery] int id, StoreDto storeDto)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var result = await unitOfWork.StoreRepository.UpdateStoreAsync(id, storeDto);

            if (!result)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteStore([FromQuery] int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("token is missing");
            }
            var result = await unitOfWork.StoreRepository.DeleteStoreAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }

}
