using Chat.API.Interfaces;
using Chat.API.Requests.Message.GetChatMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController(IMessageService messageService, IClaimsManager claimsManager) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<GetChatMessagesResponse> GetChatMessagesAsync(
        Guid chatId,
        CancellationToken cancellationToken)
        => await messageService.GetMessagesByChatIdAsync(chatId, claimsManager.GetUserId(HttpContext.User), cancellationToken);
}