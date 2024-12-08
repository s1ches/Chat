export class CreateChatRequest {
    constructor(chatName, userNames) {
        this.chatName = chatName;
        this.userNames = userNames;
    }
}