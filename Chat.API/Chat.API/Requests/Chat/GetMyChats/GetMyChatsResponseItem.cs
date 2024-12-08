namespace Chat.API.Requests.Chat.GetMyChats;

public class GetMyChatsResponseItem
{
    public Guid ChatId { get; set; }
    
    public string ChatName { get; set; } = string.Empty;
    
    public string[] ParticipantsNames { get; set; } = [];
    
    public DateTime UpdateDate { get; set; }
}