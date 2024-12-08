using System.Net;
using Chat.API.Data.Interfaces;
using Chat.API.Exceptions;
using Chat.API.Interfaces;
using Chat.API.Requests.Chat.CreateChat;
using Chat.API.Requests.Message.CreateMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hubs;

[Authorize]
public class ChatHub(
    IChatService chatService,
    IMessageService messageService,
    IConnectionManager connectionManager,
    IUnitOfWork unitOfWork,
    IClaimsManager claimsManager) : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "General");
        
        var userId = claimsManager.GetUserId(Context.User ??
                                             throw new BaseException("Unauthorized", HttpStatusCode.Unauthorized));
        
        var chatsIds = (await unitOfWork.ChatRepository.GetChatsByUserIdAsync(userId))
            .Select(x => x.Id)
            .ToList();
        
        foreach(var chatId in chatsIds)
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        
        await connectionManager.AddConnectionAsync(Context.ConnectionId, userId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "General");
        var userId = claimsManager.GetUserId(Context.User ??
                                             throw new BaseException("Unauthorized", HttpStatusCode.Unauthorized));
        
        var chatsIds = (await unitOfWork.ChatRepository.GetChatsByUserIdAsync(userId))
            .Select(x => x.Id)
            .ToList();
        
        foreach(var chatId in chatsIds)
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        
        await connectionManager.RemoveConnectionAsync(userId);
    }

    public async Task CreateChat(CreateChatRequest request)
    {
        var response = await chatService.CreateChatAsync(claimsManager.GetUserId(Context.User ??
            throw new BaseException("Unauthorized", HttpStatusCode.Unauthorized)), request);
        
        var connections = await unitOfWork.UserRepository.GetUsersConnectionsByNamesAsync(request.UserNames);
        
        foreach (var connection in connections.Where(x => x != null))
            await Groups.AddToGroupAsync(connection!, response.ChatId.ToString());
        
        await Groups.AddToGroupAsync(Context.ConnectionId, response.ChatId.ToString());
        
        await Clients.Group(response.ChatId.ToString())
            .SendAsync("NewChat", response);
    }

    public async Task SendMessage(CreateMessageRequest request)
    {
        var response =
            await messageService.CreateMessageAsync(
                claimsManager.GetUserId(Context.User ??
                                        throw new BaseException("Unauthorized", HttpStatusCode.Unauthorized)),
                request);

        await Clients.Group(request.ChatId.ToString()).SendAsync("NewMessage", response);
    }
}