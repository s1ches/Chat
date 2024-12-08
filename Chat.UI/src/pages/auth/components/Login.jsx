// eslint-disable-next-line no-unused-vars
import React, {useContext, useState} from "react";
import "../styles/Auth.css";
import {$host} from "../../../http/index.js";
import {UserContext} from "../../../main.jsx";
import {getUserInfo} from "../../../functions/jwtManager.js";

const Login = () => {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const userStore = useContext(UserContext)


    const handleSubmit = async (e) => {
        e.preventDefault();
        $host.post("/api/auth/login", {
            userName,
            password
        })
            .then((res) => {
                console.log(res.data.accessToken);
                localStorage.setItem("accessToken", res.data.accessToken);
                userStore.login(getUserInfo(res.data.accessToken));
            });
    };

    return (
        <div className="auth-container">
            <form className="auth-form" onSubmit={handleSubmit}>
                <h2>Login</h2>
                <label htmlFor="username">Username:</label>
                <input
                    type="text"
                    id="username"
                    value={userName}
                    onChange={(e) => setUserName(e.target.value)}
                    placeholder="Enter your username"
                    required
                />
                <label htmlFor="password">Password:</label>
                <input
                    type="password"
                    id="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Enter your password"
                    required
                />
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default Login;