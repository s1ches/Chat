using Chat.API.Interfaces;
using Chat.API.Requests.Chat.GetMyChats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers;

[ApiController]
[Route("api/chats")]
public class ChatController(IChatService chatService, IClaimsManager claimsManager) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<GetMyChatsResponse> GetMyChats(
        CancellationToken cancellationToken)
        => await chatService.GetUserChatsAsync(claimsManager.GetUserId(HttpContext.User), cancellationToken);
}