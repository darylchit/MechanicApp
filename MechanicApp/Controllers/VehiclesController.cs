using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public VehiclesController(
        IVehicleRepository vehicleRepository,
        IServiceRequestRepository serviceRequestRepository,
        IClientRepository clientRepository,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all vehicles
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<VehicleDto>))]
    public async Task<IActionResult> GetVehicles()
    {
        var vehicles = await _vehicleRepository.GetVehiclesAsync();
        var vehiclesDto = _mapper.Map<List<VehicleDto>>(vehicles);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(vehiclesDto);
    }

    /// <summary>
    /// Get a specific vehicle by ID
    /// </summary>
    [HttpGet("{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(VehicleDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetVehicle(int vehicleId)
    {
        if (!await _vehicleRepository.VehicleExistsAsync(vehicleId))
            return NotFound($"Vehicle with ID {vehicleId} not found.");

        var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
        var vehicleDto = _mapper.Map<VehicleDto>(vehicle);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(vehicleDto);
    }

    /// <summary>
    /// Get all vehicles for a specific client
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<VehicleDto>))]
    public async Task<IActionResult> GetVehiclesByClient(int clientId)
    {
        var vehicles = await _vehicleRepository.GetVehiclesByClientAsync(clientId);
        var vehiclesDto = _mapper.Map<List<VehicleDto>>(vehicles);
        return Ok(vehiclesDto);
    }

    /// <summary>
    /// Get all service requests for a specific vehicle
    /// </summary>
    [HttpGet("{vehicleId}/servicerequests")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetVehicleServiceRequests(int vehicleId)
    {
        if (!await _vehicleRepository.VehicleExistsAsync(vehicleId))
            return NotFound($"Vehicle with ID {vehicleId} not found.");

        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByVehicleAsync(vehicleId);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Register a new vehicle
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(VehicleDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleDto vehicleCreate)
    {
        if (vehicleCreate == null)
            return BadRequest("Vehicle data is required.");

        // Check if client exists
        if (!await _clientRepository.ClientExistsAsync(vehicleCreate.ClientId))
            return NotFound($"Client with ID {vehicleCreate.ClientId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var vehicleMap = _mapper.Map<Models.Vehicle>(vehicleCreate);

        if (!await _vehicleRepository.CreateVehicleAsync(vehicleMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the vehicle.");
            return StatusCode(500, ModelState);
        }

        var vehicleDto = _mapper.Map<VehicleDto>(vehicleMap);
        return CreatedAtAction(nameof(GetVehicle), new { vehicleId = vehicleMap.Id }, vehicleDto);
    }

    /// <summary>
    /// Update an existing vehicle
    /// </summary>
    [HttpPut("{vehicleId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateVehicle(int vehicleId, [FromBody] UpdateVehicleDto updatedVehicle)
    {
        if (updatedVehicle == null)
            return BadRequest("Vehicle data is required.");

        if (!await _vehicleRepository.VehicleExistsAsync(vehicleId))
            return NotFound($"Vehicle with ID {vehicleId} not found.");

        // Check if client exists
        if (!await _clientRepository.ClientExistsAsync(updatedVehicle.ClientId))
            return NotFound($"Client with ID {updatedVehicle.ClientId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var vehicleMap = _mapper.Map<Models.Vehicle>(updatedVehicle);
        vehicleMap.Id = vehicleId;

        if (!await _vehicleRepository.UpdateVehicleAsync(vehicleMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the vehicle.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a vehicle
    /// </summary>
    [HttpDelete("{vehicleId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteVehicle(int vehicleId)
    {
        if (!await _vehicleRepository.VehicleExistsAsync(vehicleId))
            return NotFound($"Vehicle with ID {vehicleId} not found.");

        var vehicleToDelete = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);

        if (vehicleToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _vehicleRepository.DeleteVehicleAsync(vehicleToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the vehicle.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
