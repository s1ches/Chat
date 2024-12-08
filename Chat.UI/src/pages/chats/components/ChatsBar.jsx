import React, {useState} from 'react';
import Chat from './Chat';
import "../styles/Chats.css";
import CreateChatModal from "./CreateChatModal.jsx";

const ChatBar = ({ chats, onSelectChat, onAddChat }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    // Функция для открытия модального окна
    const openModal = () => setIsModalOpen(true);
    // Функция для закрытия модального окна
    const closeModal = () => setIsModalOpen(false);

    // Функция для добавления нового чата
    const handleCreateChat = (name, participants) => {
        onAddChat(name, participants); // Добавляем новый чат
        closeModal(); // Закрываем модальное окно после создания чата
    };

    return (
        <div className="chat-bar">
            <div className="chat-list">
                {chats.map((chat, index) => (
                    <Chat key={index} name={chat.chatName} onClick={() => onSelectChat(chat)} />
                ))}
            </div>
            <div className="chat-bar-add" onClick={openModal}>
                +
            </div>

            {isModalOpen && (
                <CreateChatModal onClose={closeModal} onCreateChat={handleCreateChat} />
            )}
        </div>
    );
};

export default ChatBar;