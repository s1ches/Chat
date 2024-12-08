namespace Chat.API.Requests.Message.GetChatMessages;

public class GetChatMessagesResponse
{
    public List<GetChatMessagesResponseItem> Messages { get; set; } = [];
    
    public int TotalCount { get; set; }
}