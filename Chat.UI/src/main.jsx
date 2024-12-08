// eslint-disable-next-line no-unused-vars
import React, {createContext} from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import './App.css'
import UserStore from "./stores/userStore.js";

export const UserContext = createContext(new UserStore());

const root = ReactDOM.createRoot(document.getElementById('root'))
root.render(
    // <React.StrictMode>
        <UserContext.Provider value={new UserStore()}>
                <App/>
        </UserContext.Provider>
    // </React.StrictMode>
);
