import {useContext} from 'react';
// import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import './index.css';
// eslint-disable-next-line no-unused-vars
import React from 'react';
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./common/router/AppRouter.jsx";

const App = ()  => {
    return (
        <BrowserRouter>
            <header className="app-header">
                <div className="app-logo">💬 Chats</div>
            </header>
            <div className="app-router-container">
                <AppRouter />
            </div>
        </BrowserRouter>
    );
}

export default App
