// import React, { useState, useRef, useEffect } from 'react';
// import Message from './Message';
// import "../styles/Chats.css";
//
// const ChatWindow = ({ chat, onSendMessage }) => {
//     const [message, setMessage] = useState('');
//     const messagesContainerRef = useRef(null);
//
//     const handleSendMessage = () => {
//         if (message.trim()) {
//             onSendMessage(message);
//             setMessage('');
//         }
//     };
//
//     const handleKeyDown = (event) => {
//         if (event.key === 'Enter') {
//             event.preventDefault(); // Предотвращает перенос строки в текстовом поле
//             handleSendMessage();
//         }
//     };
//
//     // Прокрутка вниз внутри контейнера сообщений
//     const scrollToBottom = () => {
//         const container = messagesContainerRef.current;
//         if (container) {
//             container.scrollTop = container.scrollHeight;
//         }
//     };
//
//     // Прокрутка вниз при изменении сообщений
//     useEffect(() => {
//         scrollToBottom();
//     }, [chat.messages]);
//
//     return (
//         <div className="chat-window">
//             <h2>{chat.chatName}</h2>
//             <div className="messages" ref={messagesContainerRef}>
//                 {chat.messages.length !== 0 && chat.messages.map((msg, index) => (
//                     <Message key={index} sender={msg.senderName} text={msg.messageContent} date={msg.sendDate} />
//                 ))}
//             </div>
//             <div className="message-input">
//                 <input
//                     type="text"
//                     value={message}
//                     onChange={(e) => setMessage(e.target.value)}
//                     placeholder="Type a message..."
//                     onKeyDown={handleKeyDown} // Добавлен обработчик нажатия клавиш
//                 />
//                 <button onClick={handleSendMessage}>Send</button>
//             </div>
//         </div>
//     );
// };
//
// export default ChatWindow;

import React, { useState, useRef, useEffect } from "react";
import Message from "./Message";
import "../styles/Chats.css";

const ChatWindow = ({ chat, onSendMessage }) => {
    const [message, setMessage] = useState("");
    const [isModalOpen, setModalOpen] = useState(false); // Состояние для модального окна
    const messagesContainerRef = useRef(null);

    const handleSendMessage = () => {
        if (message.trim()) {
            onSendMessage(message);
            setMessage("");
        }
    };

    const handleKeyDown = (event) => {
        if (event.key === "Enter") {
            event.preventDefault();
            handleSendMessage();
        }
    };

    const scrollToBottom = () => {
        const container = messagesContainerRef.current;
        if (container) {
            container.scrollTop = container.scrollHeight;
        }
    };

    useEffect(() => {
        scrollToBottom();
    }, [chat.messages]);

    return (
        <div className="chat-window">
            {/* Заголовок чата */}
            <h2 onClick={() => setModalOpen(true)} className="chat-name">
                {chat.chatName}
            </h2>

            {/* Список сообщений */}
            <div className="messages" ref={messagesContainerRef}>
                {chat.messages.length !== 0 &&
                    chat.messages.map((msg, index) => (
                        <Message
                            key={index}
                            sender={msg.senderName}
                            text={msg.messageContent}
                            date={msg.sendDate}
                        />
                    ))}
            </div>

            <div className="message-input">
                <input
                    type="text"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    placeholder="Type a message..."
                    onKeyDown={handleKeyDown}
                />
                <button onClick={handleSendMessage}>Send</button>
            </div>

            {/* Модальное окно со списком участников */}
            {isModalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h2>Participants</h2>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                            <ul className="participant-list">
                                {chat.usernames.map((participant, index) => (
                                    <li key={index} className="participant-name">
                                        {participant}
                                    </li>
                                ))}
                            </ul>
                            <div className="modal-actions">
                                <button onClick={() => setModalOpen(false)}>Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ChatWindow;