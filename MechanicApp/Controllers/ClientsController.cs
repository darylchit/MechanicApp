using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientRepository _clientRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IMapper _mapper;

    public ClientsController(
        IClientRepository clientRepository,
        IVehicleRepository vehicleRepository,
        IServiceRequestRepository serviceRequestRepository,
        IMapper mapper)
    {
        _clientRepository = clientRepository;
        _vehicleRepository = vehicleRepository;
        _serviceRequestRepository = serviceRequestRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all clients
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ClientDto>))]
    public async Task<IActionResult> GetClients()
    {
        var clients = await _clientRepository.GetClientsAsync(); // Make sure this method includes User navigation property
        var clientsDto = _mapper.Map<List<ClientDto>>(clients); // This will automatically map the UserId from the User navigation property to the ClientDto.UserId

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(clientsDto);
    }

    /// <summary>
    /// Get a specific client by ID
    /// </summary>
    [HttpGet("{clientId}")]
    [ProducesResponseType(200, Type = typeof(ClientDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetClient(int clientId)
    {
        if (!await _clientRepository.ClientExistsAsync(clientId)) // This check is optional since we will also check for
                                                                  // null after trying to retrieve the client, but it allows us to return a more specific error message
            return NotFound($"Client with ID {clientId} not found.");

        var client = await _clientRepository.GetClientByIdAsync(clientId); // Make sure this method includes User navigation property
        var clientDto = _mapper.Map<ClientDto>(client); // This will automatically map the UserId from the User navigation property to the ClientDto.UserId

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(clientDto);
    }

    /// <summary>
    /// Get a client by their User ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(200, Type = typeof(ClientDto))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetClientByUserId(string userId)
    {
        var client = await _clientRepository.GetClientByUserIdAsync(userId);

        if (client == null)
            return NotFound($"Client with User ID {userId} not found.");

        var clientDto = _mapper.Map<ClientDto>(client);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(clientDto);
    }

    /// <summary>
    /// Get all vehicles for a specific client
    /// </summary>
    [HttpGet("{clientId}/vehicles")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<VehicleDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetClientVehicles(int clientId)
    {
        if (!await _clientRepository.ClientExistsAsync(clientId))
            return NotFound($"Client with ID {clientId} not found.");

        var vehicles = await _vehicleRepository.GetVehiclesByClientAsync(clientId);
        var vehiclesDto = _mapper.Map<List<VehicleDto>>(vehicles);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(vehiclesDto);
    }

    /// <summary>
    /// Get all service requests for a specific client
    /// </summary>
    [HttpGet("{clientId}/servicerequests")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetClientServiceRequests(int clientId)
    {
        if (!await _clientRepository.ClientExistsAsync(clientId))
            return NotFound($"Client with ID {clientId} not found.");

        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByClientAsync(clientId);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Create a new client
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ClientDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDto clientCreate)
    {
        if (clientCreate == null)
            return BadRequest("Client data is required.");

        // Check if client with this UserId already exists
        var existingClient = await _clientRepository.GetClientByUserIdAsync(clientCreate.UserId);
        if (existingClient != null)
        {
            ModelState.AddModelError("", $"Client with User ID {clientCreate.UserId} already exists.");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var clientMap = _mapper.Map<Client>(clientCreate);

        if (!await _clientRepository.CreateClientAsync(clientMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the client.");
            return StatusCode(500, ModelState);
        }

        // Reload the client with User navigation property included
        var createdClient = await _clientRepository.GetClientByIdAsync(clientMap.Id);
        var clientDto = _mapper.Map<ClientDto>(createdClient);
        return CreatedAtAction(nameof(GetClient), new { clientId = clientMap.Id }, clientDto);
    }

    /// <summary>
    /// Update an existing client
    /// </summary>
    [HttpPut("{clientId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateClient(int clientId, [FromBody] UpdateClientDto updatedClient)
    {
        if (updatedClient == null)
            return BadRequest("Client data is required.");

        if (!await _clientRepository.ClientExistsAsync(clientId))
            return NotFound($"Client with ID {clientId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var clientMap = _mapper.Map<Client>(updatedClient);
        clientMap.Id = clientId;

        if (!await _clientRepository.UpdateClientAsync(clientMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the client.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a client
    /// </summary>
    [HttpDelete("{clientId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteClient(int clientId)
    {
        if (!await _clientRepository.ClientExistsAsync(clientId))
            return NotFound($"Client with ID {clientId} not found.");

        var clientToDelete = await _clientRepository.GetClientByIdAsync(clientId);

        if (clientToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _clientRepository.DeleteClientAsync(clientToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the client.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
