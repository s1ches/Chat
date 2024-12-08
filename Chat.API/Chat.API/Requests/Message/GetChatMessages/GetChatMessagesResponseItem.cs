namespace Chat.API.Requests.Message.GetChatMessages;

public class GetChatMessagesResponseItem
{
    public Guid MessageId { get; set; }
    
    public string MessageContent { get; set; } = string.Empty;
    
    public Guid CreatorId { get; set; }
    
    public string CreatorName { get; set; } = string.Empty;
    
    public DateTime CreateDate { get; set; }
}