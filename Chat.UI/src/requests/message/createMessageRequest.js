export class CreateMessageRequest {
    constructor(messageContent, chatId) {
        this.messageContent = messageContent;
        this.chatId = chatId;
    }
}