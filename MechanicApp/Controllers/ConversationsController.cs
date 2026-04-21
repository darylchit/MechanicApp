using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConversationsController : ControllerBase
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IMechanicRepository _mechanicRepository;
    private readonly IMapper _mapper;

    public ConversationsController(
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository,
        IClientRepository clientRepository,
        IMechanicRepository mechanicRepository,
        IMapper mapper)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _clientRepository = clientRepository;
        _mechanicRepository = mechanicRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all conversations
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ConversationDto>))]
    public async Task<IActionResult> GetConversations()
    {
        var conversations = await _conversationRepository.GetConversationsAsync();
        var conversationsDto = _mapper.Map<List<ConversationDto>>(conversations);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(conversationsDto);
    }

    /// <summary>
    /// Get a specific conversation by ID
    /// </summary>
    [HttpGet("{conversationId}")]
    [ProducesResponseType(200, Type = typeof(ConversationDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetConversation(int conversationId)
    {
        if (!await _conversationRepository.ConversationExistsAsync(conversationId))
            return NotFound($"Conversation with ID {conversationId} not found.");

        var conversation = await _conversationRepository.GetConversationByIdAsync(conversationId);
        var conversationDto = _mapper.Map<ConversationDto>(conversation);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(conversationDto);
    }

    /// <summary>
    /// Get all conversations for a specific client
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ConversationDto>))]
    public async Task<IActionResult> GetConversationsByClient(int clientId)
    {
        var conversations = await _conversationRepository.GetConversationsByClientAsync(clientId);
        var conversationsDto = _mapper.Map<List<ConversationDto>>(conversations);
        return Ok(conversationsDto);
    }

    /// <summary>
    /// Get all conversations for a specific mechanic
    /// </summary>
    [HttpGet("mechanic/{mechanicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ConversationDto>))]
    public async Task<IActionResult> GetConversationsByMechanic(int mechanicId)
    {
        var conversations = await _conversationRepository.GetConversationsByMechanicAsync(mechanicId);
        var conversationsDto = _mapper.Map<List<ConversationDto>>(conversations);
        return Ok(conversationsDto);
    }

    /// <summary>
    /// Get all messages in a specific conversation
    /// </summary>
    [HttpGet("{conversationId}/messages")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageDto>))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetConversationMessages(int conversationId)
    {
        if (!await _conversationRepository.ConversationExistsAsync(conversationId))
            return NotFound($"Conversation with ID {conversationId} not found.");

        var messages = await _messageRepository.GetMessagesByConversationAsync(conversationId);
        var messagesDto = _mapper.Map<List<MessageDto>>(messages);
        return Ok(messagesDto);
    }

    /// <summary>
    /// Create a new conversation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ConversationDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDto conversationCreate)
    {
        if (conversationCreate == null)
            return BadRequest("Conversation data is required.");

        // Validate client exists
        if (!await _clientRepository.ClientExistsAsync(conversationCreate.ClientId))
            return NotFound($"Client with ID {conversationCreate.ClientId} not found.");

        // Validate mechanic exists
        if (!await _mechanicRepository.MechanicExistsAsync(conversationCreate.MechanicId))
            return NotFound($"Mechanic with ID {conversationCreate.MechanicId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var conversationMap = _mapper.Map<Models.Conversation>(conversationCreate);

        if (!await _conversationRepository.CreateConversationAsync(conversationMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the conversation.");
            return StatusCode(500, ModelState);
        }

        // Reload with navigation properties
        var createdConversation = await _conversationRepository.GetConversationByIdAsync(conversationMap.Id);
        var conversationDto = _mapper.Map<ConversationDto>(createdConversation);
        return CreatedAtAction(nameof(GetConversation), new { conversationId = conversationMap.Id }, conversationDto);
    }

    /// <summary>
    /// Update an existing conversation
    /// </summary>
    [HttpPut("{conversationId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateConversation(int conversationId, [FromBody] UpdateConversationDto updatedConversation)
    {
        if (updatedConversation == null)
            return BadRequest("Conversation data is required.");

        if (!await _conversationRepository.ConversationExistsAsync(conversationId))
            return NotFound($"Conversation with ID {conversationId} not found.");

        // Validate client exists
        if (!await _clientRepository.ClientExistsAsync(updatedConversation.ClientId))
            return NotFound($"Client with ID {updatedConversation.ClientId} not found.");

        // Validate mechanic exists
        if (!await _mechanicRepository.MechanicExistsAsync(updatedConversation.MechanicId))
            return NotFound($"Mechanic with ID {updatedConversation.MechanicId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var conversationMap = _mapper.Map<Models.Conversation>(updatedConversation);
        conversationMap.Id = conversationId;

        if (!await _conversationRepository.UpdateConversationAsync(conversationMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the conversation.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a conversation
    /// </summary>
    [HttpDelete("{conversationId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteConversation(int conversationId)
    {
        if (!await _conversationRepository.ConversationExistsAsync(conversationId))
            return NotFound($"Conversation with ID {conversationId} not found.");

        var conversationToDelete = await _conversationRepository.GetConversationByIdAsync(conversationId);

        if (conversationToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _conversationRepository.DeleteConversationAsync(conversationToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the conversation.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
