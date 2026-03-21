using AutoMapper;
using MechanicApp.DTOs;
using MechanicApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MechanicApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessagesController(
        IMessageRepository messageRepository,
        IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all messages
    /// </summary>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageDto>))]
    public async Task<IActionResult> GetMessages()
    {
        var messages = await _messageRepository.GetMessagesAsync();
        var messagesDto = _mapper.Map<List<MessageDto>>(messages);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(messagesDto);
    }

    /// <summary>
    /// Get a specific message by ID
    /// </summary>
    [HttpGet("{messageId}")]
    [ProducesResponseType(200, Type = typeof(MessageDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMessage(int messageId)
    {
        if (!await _messageRepository.MessageExistsAsync(messageId))
            return NotFound($"Message with ID {messageId} not found.");

        var message = await _messageRepository.GetMessageByIdAsync(messageId);
        var messageDto = _mapper.Map<MessageDto>(message);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(messageDto);
    }

    /// <summary>
    /// Get all messages in a specific conversation
    /// </summary>
    [HttpGet("conversation/{conversationId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageDto>))]
    public async Task<IActionResult> GetMessagesByConversation(int conversationId)
    {
        var messages = await _messageRepository.GetMessagesByConversationAsync(conversationId);
        var messagesDto = _mapper.Map<List<MessageDto>>(messages);
        return Ok(messagesDto);
    }

    /// <summary>
    /// Get unread messages for a specific user
    /// </summary>
    [HttpGet("unread/user/{userId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MessageDto>))]
    public async Task<IActionResult> GetUnreadMessagesByUser(string userId)
    {
        var messages = await _messageRepository.GetUnreadMessagesByUserAsync(userId);
        var messagesDto = _mapper.Map<List<MessageDto>>(messages);
        return Ok(messagesDto);
    }
}
