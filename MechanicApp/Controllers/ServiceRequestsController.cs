using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceRequestsController : ControllerBase
{
    private readonly IServiceRequestRepository _serviceRequestRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IMapper _mapper;

    public ServiceRequestsController(
        IServiceRequestRepository serviceRequestRepository,
        IClientRepository clientRepository,
        IVehicleRepository vehicleRepository,
        IMechanicRepository mechanicRepository,
        IMapper mapper)
    {
        _serviceRequestRepository = serviceRequestRepository;
        _clientRepository = clientRepository;
        _vehicleRepository = vehicleRepository;
        _mechanicRepository = mechanicRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all service requests
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    public async Task<IActionResult> GetServiceRequests()
    {
        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsAsync();
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Get a specific service request by ID
    /// </summary>
    [HttpGet("{serviceRequestId}")]
    [ProducesResponseType(200, Type = typeof(ServiceRequestDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetServiceRequest(int serviceRequestId)
    {
        if (!await _serviceRequestRepository.ServiceRequestExistsAsync(serviceRequestId))
            return NotFound($"Service request with ID {serviceRequestId} not found.");

        var serviceRequest = await _serviceRequestRepository.GetServiceRequestByIdAsync(serviceRequestId);
        var serviceRequestDto = _mapper.Map<ServiceRequestDto>(serviceRequest);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(serviceRequestDto);
    }

    /// <summary>
    /// Get all service requests by status
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetServiceRequestsByStatus(string status)
    {
        if (!Enum.TryParse<Models.ServiceStatus>(status, true, out var serviceStatus))
            return BadRequest($"Invalid status value: {status}. Valid values are: Pending, InProgress, Completed, Cancelled");

        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByStatusAsync(serviceStatus);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Get all service requests for a specific client
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    public async Task<IActionResult> GetServiceRequestsByClient(int clientId)
    {
        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByClientAsync(clientId);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Get all service requests for a specific mechanic
    /// </summary>
    [HttpGet("mechanic/{mechanicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    public async Task<IActionResult> GetServiceRequestsByMechanic(int mechanicId)
    {
        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByMechanicAsync(mechanicId);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Get all service requests for a specific vehicle
    /// </summary>
    [HttpGet("vehicle/{vehicleId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ServiceRequestDto>))]
    public async Task<IActionResult> GetServiceRequestsByVehicle(int vehicleId)
    {
        var serviceRequests = await _serviceRequestRepository.GetServiceRequestsByVehicleAsync(vehicleId);
        var serviceRequestsDto = _mapper.Map<List<ServiceRequestDto>>(serviceRequests);
        return Ok(serviceRequestsDto);
    }

    /// <summary>
    /// Create a new service request
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ServiceRequestDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateServiceRequest([FromBody] CreateServiceRequestDto serviceRequestCreate)
    {
        if (serviceRequestCreate == null)
            return BadRequest("Service request data is required.");

        // Validate client exists
        if (!await _clientRepository.ClientExistsAsync(serviceRequestCreate.ClientId))
            return NotFound($"Client with ID {serviceRequestCreate.ClientId} not found.");

        // Validate vehicle exists
        if (!await _vehicleRepository.VehicleExistsAsync(serviceRequestCreate.VehicleId))
            return NotFound($"Vehicle with ID {serviceRequestCreate.VehicleId} not found.");

        // Validate mechanic exists (if provided)
        if (serviceRequestCreate.MechanicId.HasValue && 
            !await _mechanicRepository.MechanicExistsAsync(serviceRequestCreate.MechanicId.Value))
            return NotFound($"Mechanic with ID {serviceRequestCreate.MechanicId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var serviceRequestMap = _mapper.Map<Models.ServiceRequest>(serviceRequestCreate);

        if (!await _serviceRequestRepository.CreateServiceRequestAsync(serviceRequestMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the service request.");
            return StatusCode(500, ModelState);
        }

        // Reload with navigation properties
        var createdServiceRequest = await _serviceRequestRepository.GetServiceRequestByIdAsync(serviceRequestMap.Id);
        var serviceRequestDto = _mapper.Map<ServiceRequestDto>(createdServiceRequest);
        return CreatedAtAction(nameof(GetServiceRequest), new { serviceRequestId = serviceRequestMap.Id }, serviceRequestDto);
    }

    /// <summary>
    /// Update an existing service request
    /// </summary>
    [HttpPut("{serviceRequestId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateServiceRequest(int serviceRequestId, [FromBody] UpdateServiceRequestDto updatedServiceRequest)
    {
        if (updatedServiceRequest == null)
            return BadRequest("Service request data is required.");

        if (!await _serviceRequestRepository.ServiceRequestExistsAsync(serviceRequestId))
            return NotFound($"Service request with ID {serviceRequestId} not found.");

        // Validate client exists
        if (!await _clientRepository.ClientExistsAsync(updatedServiceRequest.ClientId))
            return NotFound($"Client with ID {updatedServiceRequest.ClientId} not found.");

        // Validate vehicle exists
        if (!await _vehicleRepository.VehicleExistsAsync(updatedServiceRequest.VehicleId))
            return NotFound($"Vehicle with ID {updatedServiceRequest.VehicleId} not found.");

        // Validate mechanic exists (if provided)
        if (updatedServiceRequest.MechanicId.HasValue && 
            !await _mechanicRepository.MechanicExistsAsync(updatedServiceRequest.MechanicId.Value))
            return NotFound($"Mechanic with ID {updatedServiceRequest.MechanicId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var serviceRequestMap = _mapper.Map<Models.ServiceRequest>(updatedServiceRequest);
        serviceRequestMap.Id = serviceRequestId;

        if (!await _serviceRequestRepository.UpdateServiceRequestAsync(serviceRequestMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the service request.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a service request
    /// </summary>
    [HttpDelete("{serviceRequestId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteServiceRequest(int serviceRequestId)
    {
        if (!await _serviceRequestRepository.ServiceRequestExistsAsync(serviceRequestId))
            return NotFound($"Service request with ID {serviceRequestId} not found.");

        var serviceRequestToDelete = await _serviceRequestRepository.GetServiceRequestByIdAsync(serviceRequestId);

        if (serviceRequestToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _serviceRequestRepository.DeleteServiceRequestAsync(serviceRequestToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the service request.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
