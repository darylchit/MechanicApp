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
    private readonly IConversationRepository _conversationRepository;
    private readonly IMapper _mapper;

    public MessagesController(
        IMessageRepository messageRepository,
        IConversationRepository conversationRepository,
        IMapper mapper)
    {
        _messageRepository = messageRepository;
        _conversationRepository = conversationRepository;
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

    /// <summary>
    /// Send a new message
    /// </summary>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(MessageDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto messageCreate)
    {
        if (messageCreate == null)
            return BadRequest("Message data is required.");

        // Validate conversation exists
        if (!await _conversationRepository.ConversationExistsAsync(messageCreate.ConversationId))
            return NotFound($"Conversation with ID {messageCreate.ConversationId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var messageMap = _mapper.Map<Models.Message>(messageCreate);

        if (!await _messageRepository.CreateMessageAsync(messageMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving the message.");
            return StatusCode(500, ModelState);
        }

        // Reload with navigation properties
        var createdMessage = await _messageRepository.GetMessageByIdAsync(messageMap.Id);
        var messageDto = _mapper.Map<MessageDto>(createdMessage);
        return CreatedAtAction(nameof(GetMessage), new { messageId = messageMap.Id }, messageDto);
    }

    /// <summary>
    /// Update an existing message
    /// </summary>
    [HttpPut("{messageId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateMessage(int messageId, [FromBody] UpdateMessageDto updatedMessage)
    {
        if (updatedMessage == null)
            return BadRequest("Message data is required.");

        if (!await _messageRepository.MessageExistsAsync(messageId))
            return NotFound($"Message with ID {messageId} not found.");

        // Validate conversation exists
        if (!await _conversationRepository.ConversationExistsAsync(updatedMessage.ConversationId))
            return NotFound($"Conversation with ID {updatedMessage.ConversationId} not found.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var messageMap = _mapper.Map<Models.Message>(updatedMessage);
        messageMap.Id = messageId;

        if (!await _messageRepository.UpdateMessageAsync(messageMap))
        {
            ModelState.AddModelError("", "Something went wrong while updating the message.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a message
    /// </summary>
    [HttpDelete("{messageId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteMessage(int messageId)
    {
        if (!await _messageRepository.MessageExistsAsync(messageId))
            return NotFound($"Message with ID {messageId} not found.");

        var messageToDelete = await _messageRepository.GetMessageByIdAsync(messageId);

        if (messageToDelete == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _messageRepository.DeleteMessageAsync(messageToDelete))
        {
            ModelState.AddModelError("", "Something went wrong while deleting the message.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    /// <summary>
    /// Mark a message as read
    /// </summary>
    [HttpPatch("{messageId}/read")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> MarkMessageAsRead(int messageId)
    {
        if (!await _messageRepository.MessageExistsAsync(messageId))
            return NotFound($"Message with ID {messageId} not found.");

        if (!await _messageRepository.MarkAsReadAsync(messageId))
        {
            ModelState.AddModelError("", "Something went wrong while marking the message as read.");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
