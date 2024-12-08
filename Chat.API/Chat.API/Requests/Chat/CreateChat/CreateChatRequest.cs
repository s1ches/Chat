namespace Chat.API.Requests.Chat.CreateChat;

public class CreateChatRequest
{
    public string ChatName { get; set; } = string.Empty;

    public string[] UserNames { get; set; } = [];
}