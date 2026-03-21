using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShopsController : ControllerBase
{
    private readonly IShopRepository _shopRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IMapper _mapper;

    public ShopsController(
        IShopRepository shopRepository,
        IMechanicRepository mechanicRepository,
        IMapper mapper)
    {
        _shopRepository = shopRepository;
        _mechanicRepository = mechanicRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all shops
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ShopDto>))]
    public async Task<IActionResult> GetShops()
    {
        var shops = await _shopRepository.GetShopsAsync();
        var shopsDto = _mapper.Map<List<ShopDto>>(shops);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(shopsDto);
    }

    /// <summary>
    /// Get a specific shop by ID
    /// </summary>
    [HttpGet("{shopId}")]
    [ProducesResponseType(200, Type = typeof(ShopDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetShop(int shopId)
    {
        if (!await _shopRepository.ShopExistsAsync(shopId))
            return NotFound($"Shop with ID {shopId} not found.");

        var shop = await _shopRepository.GetShopByIdAsync(shopId);
        var shopDto = _mapper.Map<ShopDto>(shop);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(shopDto);
    }

    /// <summary>
    /// Get all mechanics in a specific shop
    /// </summary>
    [HttpGet("{shopId}/mechanics")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MechanicDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetShopMechanics(int shopId)
    {
        if (!await _shopRepository.ShopExistsAsync(shopId))
            return NotFound($"Shop with ID {shopId} not found.");

        var mechanics = await _mechanicRepository.GetMechanicsByShopAsync(shopId);
        var mechanicsDto = _mapper.Map<List<MechanicDto>>(mechanics);
        return Ok(mechanicsDto);
    }

    /// <summary>
    /// Create a new shop
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ShopDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateShop([FromBody] CreateShopDto shopCreate)
    {
        if (shopCreate == null)
            return BadRequest("Shop data is required.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var shopMap = _mapper.Map<Models.Shop>(shopCreate);

        if (!await _shopRepository.CreateShopAsync(shopMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the shop.");
            return StatusCode(500, ModelState);
        }

        var shopDto = _mapper.Map<ShopDto>(shopMap);
        return CreatedAtAction(nameof(GetShop), new { shopId = shopMap.Id }, shopDto);
    }

    /// <summary>
    /// Update an existing shop
    /// </summary>
    [HttpPut("{shopId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateShop(int shopId, [FromBody] UpdateShopDto updatedShop)
    {
        if (updatedShop == null)
            return BadRequest("Shop data is required.");

        if (!await _shopRepository.ShopExistsAsync(shopId))
            return NotFound($"Shop with ID {shopId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var shopMap = _mapper.Map<Models.Shop>(updatedShop);
        shopMap.Id = shopId;

        if (!await _shopRepository.UpdateShopAsync(shopMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the shop.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a shop
    /// </summary>
    [HttpDelete("{shopId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteShop(int shopId)
    {
        if (!await _shopRepository.ShopExistsAsync(shopId))
            return NotFound($"Shop with ID {shopId} not found.");

        var shopToDelete = await _shopRepository.GetShopByIdAsync(shopId);

        if (shopToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _shopRepository.DeleteShopAsync(shopToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the shop.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
