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
    private readonly IMapper _mapper;

    public ConversationsController(
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository,
        IMapper mapper)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
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
}
