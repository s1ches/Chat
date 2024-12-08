namespace Chat.API.Requests.Chat.GetMyChats;

public class GetMyChatsResponse
{
    public int TotalCount { get; set; }

    public List<GetMyChatsResponseItem> Chats { get; set; } = [];
}