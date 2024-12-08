namespace Chat.API.Requests.Message.CreateMessage;

public class CreateMessageRequest
{
    public string MessageContent { get; set; } = string.Empty;
    
    public Guid ChatId { get; set; }
}