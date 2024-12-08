// import { useEffect, useRef } from 'react';
// import React from 'react';
//
//
// const MessageContainer = ({ messages }) => {
//     const messageRef = useRef();
//
//     useEffect(() => {
//         if (messageRef && messageRef.current) {
//             const { scrollHeight, clientHeight } = messageRef.current;
//             messageRef.current.scrollTo({ left: 0, top: scrollHeight - clientHeight, behavior: 'smooth' });
//         }
//     }, [messages]);
//
//     return (
//         <div ref={messageRef} className='message-container'>
//         {messages.filter((value, index, self) =>
//             index === self.findIndex((t) => (
//                 t.id === value.id
//             ))).map((m, index) =>
//             <div key={index} className='user-message'>
//                 <div className='message bg-primary'>{m.message}</div>
//                 <div className='from-user'>{m.user}</div>
//             </div>
//         )}
//     </div>
//     );
// }
//
// export default MessageContainer;