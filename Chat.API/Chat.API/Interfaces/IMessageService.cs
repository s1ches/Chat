using Chat.API.Requests.Message.CreateMessage;
using Chat.API.Requests.Message.GetChatMessages;

namespace Chat.API.Interfaces;

public interface IMessageService
{
    Task<CreateMessageResponse> CreateMessageAsync(Guid creatorId, CreateMessageRequest request,
        CancellationToken cancellationToken = default);
    
    Task<GetChatMessagesResponse> GetMessagesByChatIdAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
}