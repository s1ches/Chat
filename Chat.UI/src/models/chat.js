export default class Chat {
    chatId;
    chatName;
    usernames;
    messages;
    updateDate;

    constructor(chatId, chatName, messages, usernames, updateDate) {
        this.chatId = chatId;
        this.chatName = chatName;
        this.usernames = usernames;
        this.messages = messages;
        this.updateDate = updateDate;
    }
}