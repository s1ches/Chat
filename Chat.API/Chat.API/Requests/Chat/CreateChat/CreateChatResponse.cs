namespace Chat.API.Requests.Chat.CreateChat;

public class CreateChatResponse
{
    public Guid ChatId { get; set; }

    public string ChatName { get; set; } = string.Empty;
    public string[] ParticipantsNames { get; set; } = [];
    
    public DateTime CreateDate { get; set; } = DateTime.Now;
}