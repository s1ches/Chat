import React, { useState } from "react";
import Login from "./components/Login";
import Register from "./components/Register";
import "./styles/Auth.css";

const Auth = () => {
    const [isLogin, setIsLogin] = useState(true); // Toggle between Login and Register

    return (
        <div className="auth-container">
            <div className="auth-toggle">
                <button
                    className={isLogin ? "active" : ""}
                    onClick={() => setIsLogin(true)}
                >
                    Login
                </button>
                <button
                    className={!isLogin ? "active" : ""}
                    onClick={() => setIsLogin(false)}
                >
                    Register
                </button>
            </div>
            <div className="auth-component">
                {isLogin ? <Login /> : <Register />}
            </div>
        </div>
    );
};

export default Auth;