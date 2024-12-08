using System.Net;
using Chat.API.Data.Interfaces;
using Chat.API.Domain.Entities;
using Chat.API.Exceptions;
using Chat.API.Interfaces;
using Chat.API.Requests.Message.CreateMessage;
using Chat.API.Requests.Message.GetChatMessages;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Services;

public class MessageService(IUnitOfWork unitOfWork) : IMessageService
{
    public async Task<CreateMessageResponse> CreateMessageAsync(Guid creatorId, CreateMessageRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.MessageContent))
            throw new BaseException("Empty message content", HttpStatusCode.BadRequest);

        var creator = await unitOfWork.UserRepository.GetByIdAsync(creatorId, cancellationToken);
        if (creator == null)
            throw new BaseException("User with id " + creatorId + " is not found", HttpStatusCode.BadRequest);

        var chat = await unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, cancellationToken);
        if (chat == null)
            throw new BaseException("Chat with id " + request.ChatId + " is not found", HttpStatusCode.BadRequest);


        var message = new Message
        {
            Id = Guid.NewGuid(),
            UserId = creatorId,
            ChatId = request.ChatId,
            Chat = chat,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
            MessageContent = request.MessageContent,
        };
        
        var executionStrategy = unitOfWork.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () => 
        { 
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                await unitOfWork.MessageRepository.CreateAsync(message, cancellationToken);
            
                chat.UpdateDate = DateTime.UtcNow;
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
        
        return new CreateMessageResponse
        {
            CreatorId = creator.Id,
            CreateDate = DateTime.UtcNow,
            ChatId = chat.Id,
            MessageId = message.Id,
            MessageContent = message.MessageContent,
            CreatorName = creator.UserName
        };
    }

    public async Task<GetChatMessagesResponse> GetMessagesByChatIdAsync(Guid chatId, Guid userId, CancellationToken cancellationToken)
    {
        if(!await unitOfWork.ChatRepository.ExistsByIdAsync(chatId, cancellationToken))
            throw new BaseException("Chat with id " + chatId + " is not found", HttpStatusCode.NotFound);
        
        if(!await unitOfWork.ChatRepository.ExistsUserInChatAsync(chatId, userId, cancellationToken))
            throw new BaseException("User with id " + userId + " is not in chat", HttpStatusCode.Forbidden);
        
        var messages = 
            (await unitOfWork.MessageRepository.GetByChatIdAsync(chatId, cancellationToken)).ToList();

        var responseItems = messages.Select(x => new GetChatMessagesResponseItem
        {
            MessageId = x.Id,
            CreateDate = x.UpdateDate,
            MessageContent = x.MessageContent,
            CreatorId = x.UserId,
            CreatorName = x.User.UserName,
        }).ToList();

        return new GetChatMessagesResponse
        {
            Messages = responseItems,
            TotalCount = responseItems.Count
        };
    }
}