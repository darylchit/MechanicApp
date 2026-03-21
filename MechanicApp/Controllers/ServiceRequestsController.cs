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
    private readonly IMapper _mapper;

    public ServiceRequestsController(
        IServiceRequestRepository serviceRequestRepository,
        IMapper mapper)
    {
        _serviceRequestRepository = serviceRequestRepository;
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
}
