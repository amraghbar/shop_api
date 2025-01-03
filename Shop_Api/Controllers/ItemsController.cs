﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Api.HF;
using Shop_Core.DTOS.Items;
using Shop_Core.Interfaces;
using Shop_Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IUnitOfWork unitOfWork;

    public ItemsController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }



    [HttpGet]
    public async Task<ActionResult<PagedResponse<ItemsDTO>>> GetItems()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("token is missing");
        }
        var userId = ExtractClaims.EtractUserId(token);
        if (!userId.HasValue)
        {
            return Unauthorized("invalid user token");
        }
        var items = await unitOfWork.ItemsRepository.GetItemsAsync();

        if (items == null )
        {
            return NotFound(new { message = "No items found." });
        }

        return Ok(items);
    }
    [HttpPost]
    public async Task<ActionResult<ItemsDTO>> AddItem([FromBody] ItemsDTO newItemDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("token is missing");
        }
        var userId = ExtractClaims.EtractUserId(token);
        if (!userId.HasValue)
        {
            return Unauthorized("invalid user token");
        }
        if (newItemDto == null)
        {
            return BadRequest(new { message = "Invalid item data." });
        }

        var newItem = MapDtoToEntity(newItemDto);

       
       

        var addedItem = await unitOfWork.ItemsRepository.AddItemAsync(newItem);
        await unitOfWork.saveAsync();

        return CreatedAtAction(nameof(GetItems), new { id = addedItem.Id }, addedItem);
    }

    [HttpPut]
    public async Task<ActionResult<ItemsDTO>> UpdateItem([FromQuery] int id, [FromBody] ItemsDTO updatedItemDto)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("token is missing");
        }
        var userId = ExtractClaims.EtractUserId(token);
        if (!userId.HasValue)
        {
            return Unauthorized("invalid user token");
        }
        if (updatedItemDto == null)
        {
            return BadRequest(new { message = "Invalid updated item data." });
        }

        var updatedItem = MapDtoToEntity(updatedItemDto);

        try
        {
            var result = await unitOfWork.ItemsRepository.UpdateItemAsync(id, updatedItem);
            await unitOfWork.saveAsync();
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteItem([FromQuery]int id)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("token is missing");
        }
        try
        {
            var userId = ExtractClaims.EtractUserId(token);
            if (!userId.HasValue)
            {
                return Unauthorized("invalid user token");
            }

            var result = await unitOfWork.ItemsRepository.DeleteItemAsync(id);
            await unitOfWork.saveAsync();
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private Items MapDtoToEntity(ItemsDTO dto)
    {
        return new Items
        {
            Name = dto.Name,
            Description = dto.Description,
            price = dto.price,
            MG_Id = dto.MG_Id,
            Sub_Id = dto.Sub_Id,
            ItemsUnits = dto.ItemUnits?.Select(unitName => new ItemsUnits { Units = new Units { Name = unitName } }).ToList(),
            InvItemStores = dto.Stores?.Select(storeDto => new InvItemStores
            {
                Balance = storeDto.Balance,
                Stores = new Stores { Name = storeDto.StoreName  }

            }).ToList(),
        };
    }

}
