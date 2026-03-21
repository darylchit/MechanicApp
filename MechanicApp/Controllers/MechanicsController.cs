using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MechanicsController : ControllerBase
{
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IMapper _mapper;

    public MechanicsController(
        IMechanicRepository mechanicRepository,
        IServiceRequestRepository serviceRequestRepository,
        IMapper mapper)
    {
        _mechanicRepository = mechanicRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all mechanics
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MechanicDto>))]
    public async Task<IActionResult> GetMechanics()
    {
        var mechanics = await _mechanicRepository.GetMechanicsAsync();
        var mechanicsDto = _mapper.Map<List<MechanicDto>>(mechanics);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(mechanicsDto);
    }

    /// <summary>
    /// Get a specific mechanic by ID
    /// </summary>
    [HttpGet("{mechanicId}")]
    [ProducesResponseType(200, Type = typeof(MechanicDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMechanic(int mechanicId)
    {
        if (!await _mechanicRepository.MechanicExistsAsync(mechanicId))
            return NotFound($"Mechanic with ID {mechanicId} not found.");

        var mechanic = await _mechanicRepository.GetMechanicByIdAsync(mechanicId);
        var mechanicDto = _mapper.Map<MechanicDto>(mechanic);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(mechanicDto);
    }

    /// <summary>
    /// Get mechanic by User ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(200, Type = typeof(MechanicDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMechanicByUserId(string userId)
    {
        var mechanic = await _mechanicRepository.GetMechanicByUserIdAsync(userId);
        
        if (mechanic == null)
            return NotFound($"Mechanic with User ID {userId} not found.");

        var mechanicDto = _mapper.Map<MechanicDto>(mechanic);
        return Ok(mechanicDto);
    }

    /// <summary>
    /// Get all mechanics in a specific shop
    /// </summary>
    [HttpGet("shop/{shopId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MechanicDto>))]
    public async Task<IActionResult> GetMechanicsByShop(int shopId)
    {
        var mechanics = await _mechanicRepository.GetMechanicsByShopAsync(shopId);
        var mechanicsDto = _mapper.Map<List<MechanicDto>>(mechanics);
        return Ok(mechanicsDto);
    }

    /// <summary>
    /// Get all available mechanics
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MechanicDto>))]
    public async Task<IActionResult> GetAvailableMechanics()
    {
        var mechanics = await _mechanicRepository.GetAvailableMechanicsAsync();
        var mechanicsDto = _mapper.Map<List<MechanicDto>>(mechanics);
        return Ok(mechanicsDto);
    }

    /// <summary>
    /// Get all service requests for a specific mechanic
    /// </summary>
    [HttpGet("{mechanicId}/servicerequests")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMechanicServiceRequests(int mechanicId)
    {
        if (!await _mechanicRepository.MechanicExistsAsync(mechanicId))
            return NotFound($"Mechanic with ID {mechanicId} not found.");

        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByMechanicAsync(mechanicId);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Create a new mechanic
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(MechanicDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateMechanic([FromBody] CreateMechanicDto mechanicCreate)
    {
        if (mechanicCreate == null)
            return BadRequest("Mechanic data is required.");

        // Check if mechanic with this UserId already exists
        var existingMechanic = await _mechanicRepository.GetMechanicByUserIdAsync(mechanicCreate.UserId);
        if (existingMechanic != null)
        {
            ModelState.AddModelError("", $"Mechanic with User ID {mechanicCreate.UserId} already exists.");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var mechanicMap = _mapper.Map<Models.Mechanic>(mechanicCreate);

        if (!await _mechanicRepository.CreateMechanicAsync(mechanicMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the mechanic.");
            return StatusCode(500, ModelState);
        }

        // Reload the mechanic with User and Shop navigation properties included
        var createdMechanic = await _mechanicRepository.GetMechanicByIdAsync(mechanicMap.Id);
        var mechanicDto = _mapper.Map<MechanicDto>(createdMechanic);
        return CreatedAtAction(nameof(GetMechanic), new { mechanicId = mechanicMap.Id }, mechanicDto);
    }

    /// <summary>
    /// Update an existing mechanic
    /// </summary>
    [HttpPut("{mechanicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateMechanic(int mechanicId, [FromBody] UpdateMechanicDto updatedMechanic)
    {
        if (updatedMechanic == null)
            return BadRequest("Mechanic data is required.");

        if (!await _mechanicRepository.MechanicExistsAsync(mechanicId))
            return NotFound($"Mechanic with ID {mechanicId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var mechanicMap = _mapper.Map<Models.Mechanic>(updatedMechanic);
        mechanicMap.Id = mechanicId;

        if (!await _mechanicRepository.UpdateMechanicAsync(mechanicMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the mechanic.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a mechanic
    /// </summary>
    [HttpDelete("{mechanicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteMechanic(int mechanicId)
    {
        if (!await _mechanicRepository.MechanicExistsAsync(mechanicId))
            return NotFound($"Mechanic with ID {mechanicId} not found.");

        var mechanicToDelete = await _mechanicRepository.GetMechanicByIdAsync(mechanicId);

        if (mechanicToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _mechanicRepository.DeleteMechanicAsync(mechanicToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the mechanic.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
