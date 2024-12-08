using System.Net;
using Chat.API.Data;
using Chat.API.Data.Interfaces;
using Chat.API.Domain.Entities;
using Chat.API.Exceptions;
using Chat.API.Interfaces;
using Chat.API.Requests.Chat.CreateChat;
using Chat.API.Requests.Chat.GetMyChats;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services;

public class ChatService(IUnitOfWork unitOfWork, ChatDbContext dataContext) : IChatService
{
    public async Task<GetMyChatsResponse> GetUserChatsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var chats = await unitOfWork.ChatRepository
            .GetChatsByUserIdAsync(userId, cancellationToken: cancellationToken);
        
        var responseItems = chats
            .Select(x => new GetMyChatsResponseItem
            {
                ChatId = x.Id,
                ChatName = x.Name,
                UpdateDate = x.UpdateDate,
                ParticipantsNames = x.Users.Select(x => x.UserName).ToArray()
            }).ToList();

        return new GetMyChatsResponse
        {
            Chats = responseItems,
            TotalCount = chats.Count
        };
    }

    public async Task<CreateChatResponse> CreateChatAsync(Guid creatorId, CreateChatRequest request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(request.ChatName))
            throw new BaseException("Chat name is required", HttpStatusCode.BadRequest);
        
        var users = await unitOfWork.UserRepository
            .GetUsersByNamesAsync(request.UserNames, cancellationToken);
     
        var creator = await unitOfWork.UserRepository.GetByIdAsync(creatorId, cancellationToken);
        
        if(creator == null)
            throw new BaseException("Invalid user id", HttpStatusCode.Forbidden);
        
        users.Add(creator);
        
        var chat = new Domain.Entities.Chat
        {
            Id = Guid.NewGuid(),
            Name = request.ChatName,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
        };

        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await unitOfWork.ChatRepository.CreateAsync(chat, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                
                chat.Users.AddRange(users);
                await unitOfWork.ChatRepository.UpdateAsync(chat, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            } 
        });

        return new CreateChatResponse
        {
            ChatId = chat.Id,
            ChatName = chat.Name,
            ParticipantsNames = users.Select(x => x.UserName).ToArray(),
            CreateDate = chat.CreateDate
        };
    }
}