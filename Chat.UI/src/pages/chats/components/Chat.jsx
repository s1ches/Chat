// eslint-disable-next-line no-unused-vars
import React from 'react';
import "../styles/Chats.css";


const Chat = ({ name, onClick }) => {
    return (
        <div className="chat" onClick={onClick}>
            <span>{name}</span>
        </div>
    );
};

export default Chat;