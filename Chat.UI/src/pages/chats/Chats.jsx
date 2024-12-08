// eslint-disable-next-line no-unused-vars
import React, { useEffect, useState, useRef } from "react";
import ChatBar from "./components/ChatsBar";
import ChatWindow from "./components/ChatWindow";
import "./styles/Chats.css";
import { $authHost } from "../../http/index.js";
import Chat from "../../models/chat.js";
import Message from "../../models/message.js";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const Chats = () => {
    const [selectedChat, setSelectedChat] = useState(null); // Выбранный чат
    const [chats, setChats] = useState([]); // Список чатов
    const [connection, setConnection] = useState(null); // SignalR соединение

    useEffect(() => {
        const checkAndRefreshConnection = async () => {
            console.log("check connection");
            if(connection?.state !== "Connected") {
                await setupConnection();
            }
        }

        const interval = setInterval(() => {
            checkAndRefreshConnection();
        }, 60 * 1000);

        checkAndRefreshConnection();

        return () => clearInterval(interval); // Очистка интервала при размонтировании
    }, [connection]);

    // useRef для хранения актуального состояния
    const chatsRef = useRef([]);
    const selectedChatRef = useRef(null);

    useEffect(() => {
        chatsRef.current = chats;
    }, [chats]);

    useEffect(() => {
        selectedChatRef.current = selectedChat;
    }, [selectedChat]);

    // Загрузка чатов с сервера
    const loadChats = async () => {
        try {
            const response = await $authHost.get("/api/chats");
            const fetchedChats = response.data.chats.map(
                (chat) => new Chat(chat.chatId, chat.chatName, [], chat.participantsNames, chat.updateDate)
            );
            setChats(fetchedChats.sort((a, b) => new Date(b.updateDate) - new Date(a.updateDate)));
        } catch (error) {
            console.error("Error loading chats:", error);
        }
    };

    // Установка SignalR подключения
    const setupConnection = async () => {
        try {
            const newConnection = new HubConnectionBuilder()
                .withUrl("http://localhost:5128/chat-hub", {
                    accessTokenFactory: () => localStorage.getItem("accessToken"),
                })
                .configureLogging(LogLevel.Information)
                .build();

            newConnection.on("NewMessage", (message) => handleNewMessage(message));
            newConnection.on("NewChat", (chat) => handleNewChat(chat));

            await newConnection.start();
            setConnection(newConnection);
        } catch (error) {
            console.error("Error setting up SignalR connection:", error);
        }
    };

    // Обработчик нового сообщения
    const handleNewMessage = (message) => {
        if (!message || !message.chatId) {
            console.warn("Invalid message received:", message);
            return;
        }

        setChats((prevChats) => {
            return prevChats.map((chat) => {
                if (chat.chatId === message.chatId) {
                    const updatedMessages = [
                        ...chat.messages,
                        new Message(
                            message.chatId,
                            message.messageContent,
                            message.creatorId,
                            message.creatorName,
                            message.createDate
                        ),
                    ].sort((a, b) => new Date(a.sendDate) - new Date(b.sendDate));

                    return new Chat(chat.chatId, chat.chatName, updatedMessages, chat.usernames, message.createDate);
                }
                return chat;
            }).sort((a, b) => new Date(b.updateDate) - new Date(a.updateDate));
        });

        // Если сообщение относится к текущему чату
        if (selectedChatRef.current?.chatId === message.chatId) {
            setSelectedChat((prevChat) => ({
                ...prevChat,
                messages: [
                    ...prevChat.messages,
                    new Message(
                        message.chatId,
                        message.messageContent,
                        message.creatorId,
                        message.creatorName,
                        message.createDate
                    ),
                ].sort((a, b) => new Date(a.send) - new Date(b.sendDate)),
            }));
        }
    };

    // Обработчик нового чата
    const handleNewChat = (chat) => {
        if (!chat || !chat.chatId) {
            console.warn("Invalid chat received:", chat);
            return;
        }

        setChats((prevChats) => {
            const newChat = new Chat(chat.chatId, chat.chatName, [], chat.participantsNames, chat.createDate);
            return [...prevChats, newChat].sort((a, b) => new Date(b.updateDate) - new Date(a.updateDate));
        });
    };

    // Загрузка сообщений для чата
    const loadMessages = async (chat) => {
        try {
            const response = await $authHost.get(`/api/messages/?chatId=${chat.chatId}`);
            const messages = response.data.messages.map(
                (m) =>
                    new Message(
                        m.chatId,
                        m.messageContent,
                        m.creatorId,
                        m.creatorName,
                        m.createDate
                    )
            );
            return messages.sort((a, b) => new Date(a.sendDate) - new Date(b.sendDate));
        } catch (error) {
            console.error("Error loading messages:", error);
            return [];
        }
    };

    // Выбор чата
    const handleSelectChat = async (chat) => {
        if (!chat) return;

        // Если сообщений нет, загружаем их с сервера
        const messages = await loadMessages(chat);
        setChats((prevChats) =>
            prevChats.map((c) =>
                c.chatId === chat.chatId ? { ...c, messages } : c
            )
        );
        setSelectedChat({ ...chat, messages });
    };

    const sendMessage = async (message) => {
        try {
            await connection.invoke("SendMessage", {
                chatId: selectedChatRef.current.chatId,
                messageContent: message,
            });
        } catch (error) {
            console.error("Error sending message:", error);
        }
    }

    // Отправка сообщения
    const handleSendMessage = async (message) => {
        if (selectedChatRef.current && connection?.state === "Connected") {
            sendMessage(message);
        }  else if(connection?.state !== "Connected") {
            setupConnection()
                .then(() => sendMessage(message));
        }
    };

        // Добавление нового чата (заглушка)
    const handleAddChat = (name, participants) => {
        if (connection) {
            if (connection.state === "Connected") {
                try {
                    connection.invoke("CreateChat", {
                        chatName: name,
                        userNames: participants,
                    });
                } catch (err) {
                    console.error("Error sending message:", err);
                }
            } else {
                console.warn("Connection is not in the 'Connected' state.");
            }
        } else {
            console.error("Connection is null or no chat is selected");
        }
    };

    useEffect(() => {
        loadChats();
    }, []);

    return (
        <div className="chats-container">
            <ChatBar chats={chats} onSelectChat={handleSelectChat} onAddChat={handleAddChat} />
            {selectedChat ? (
                <ChatWindow chat={selectedChat} onSendMessage={handleSendMessage} />
            ) : (
                <div className="no-chat">Select a chat to start messaging</div>
            )}
        </div>
    );
};

export default Chats;