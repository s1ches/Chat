export default class Message {
    chatId;
    messageContent;
    senderId;
    senderName;
    sendDate;

    constructor(chatId, messageContent, senderId, senderName, sendDate) {
        this.chatId = chatId;
        this.senderName = senderName;
        this.senderId = senderId;
        this.senderName = senderName;
        this.messageContent = messageContent;
        this.sendDate = sendDate;
    }
}