import React, {useContext} from 'react';
import "../styles/Chats.css";
import {UserContext} from "../../../main.jsx";


const Message = ({ sender, text, date }) => {
    const userStore = useContext(UserContext);
    const senderName = userStore.user.username;
    const dateModel = new Date(date).toLocaleTimeString().split(':')

    return (
        <div className={sender === senderName ? "message my-message" : "message"}>
            <span className="message-sender">{sender}</span>
            <p className="message-text">{text}</p>
            <div className="message-footer">
                {dateModel[0]+':'+dateModel[1]}
            </div>
        </div>
    );
};

export default Message;