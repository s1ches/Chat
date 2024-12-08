// eslint-disable-next-line no-unused-vars
import React, {useContext, useState} from "react";
import "../styles/Auth.css";
import {$host} from "../../../http/index.js";
import {getUserInfo} from "../../../functions/jwtManager.js";
import {UserContext} from "../../../main.jsx";

const Register = () => {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [repeatedPassword, setRepeatedPassword] = useState("");
    const userStore = useContext(UserContext)


    const handleSubmit = (e) => {
        e.preventDefault();
        if (password !== repeatedPassword) {
            alert("Passwords do not match!");
            return;
        }
        $host.post("/api/auth/register", {
            userName,
            password,
            repeatedPassword
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
                <h2>Register</h2>
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
                <label htmlFor="repeatPassword">Repeat Password:</label>
                <input
                    type="password"
                    id="repeatPassword"
                    value={repeatedPassword}
                    onChange={(e) => setRepeatedPassword(e.target.value)}
                    placeholder="Repeat your password"
                    required
                />
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default Register;