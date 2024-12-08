using Chat.API.Requests.Chat.CreateChat;
using Chat.API.Requests.Chat.GetMyChats;

namespace Chat.API.Interfaces;

public interface IChatService
{
    Task<GetMyChatsResponse> GetUserChatsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<CreateChatResponse> CreateChatAsync(Guid creatorId, CreateChatRequest request,
        CancellationToken cancellationToken = default);
}