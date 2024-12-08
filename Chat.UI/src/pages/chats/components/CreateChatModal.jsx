import React, { useState } from 'react';

const CreateChatModal = ({ onClose, onCreateChat }) => {
    const [chatName, setChatName] = useState('');
    const [participants, setParticipants] = useState('');

    // Обработчик отправки формы
    const handleSubmit = () => {
        const participantList = participants.split(',').map((name) => name.trim());
        onCreateChat(chatName, participantList);
    };

    return (
        <div className="modal-overlay">
            <div className="modal">
                <h2>Create New Chat</h2>
                <div className="form-group">
                    <label>Chat Name:</label>
                    <input
                        type="text"
                        value={chatName}
                        onChange={(e) => setChatName(e.target.value)}
                        placeholder="Enter chat name"
                    />
                </div>
                <div className="form-group">
                    <label>Participants (comma-separated):</label>
                    <input
                        type="text"
                        value={participants}
                        onChange={(e) => setParticipants(e.target.value)}
                        placeholder="Enter participants"
                    />
                </div>
                <div className="modal-actions">
                    <button onClick={handleSubmit}>Create</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
};

export default CreateChatModal;