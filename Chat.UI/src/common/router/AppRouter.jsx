import {Route, Routes} from "react-router-dom";
import {useContext, useEffect} from "react";
import {observer} from "mobx-react-lite";
import {UserContext} from "../../main.jsx";
import {authRoutes, notAuthRoutes} from "./routes.js";
import Auth from "../../pages/auth/Auth.jsx";
import Chats from "../../pages/chats/Chats.jsx";
import {getUserInfo, isExpired, needUpdate} from "../../functions/jwtManager.js";
import {$authHost} from "../../http/index.js";

const AppRouter = observer(() => {
    const userStore = useContext(UserContext);
    if(localStorage.getItem("accessToken") && !isExpired(localStorage.getItem("accessToken"))) {
        userStore.login(getUserInfo(localStorage.getItem("accessToken")));
    }

    useEffect(() => {
        const checkAndRefreshToken = async () => {
            const accessToken = localStorage.getItem("accessToken");

            if (accessToken && needUpdate(accessToken)) {
                try {
                    $authHost.post("/api/auth/refresh")
                        .then(res => {
                            console.log(res.data);
                            localStorage.setItem("accessToken", res.data.accessToken);
                        })

                } catch (error) {
                    console.error("Failed to refresh token", error);
                    userStore.logout();
                    localStorage.removeItem("accessToken");
                }
            } else if (accessToken && !userStore.isAuth) {
                userStore.login(getUserInfo(accessToken));
            }
        };

        const interval = setInterval(() => {
            checkAndRefreshToken();
        }, 2 * 60 * 1000);

        checkAndRefreshToken();

        return () => clearInterval(interval); // Очистка интервала при размонтировании
    }, [userStore]);

    return (
        <Routes>
            {
                !userStore.isAuth && notAuthRoutes.map(({path, Component}) => (
                    <Route path={path} element={<Component/>} key={path}/>
                ))
            }
            {
                userStore.isAuth && authRoutes.map(({path, Component}) => (
                    <Route path={path} element={<Component/>} key={path}/>
                ))
            }
            {
                !userStore.isAuth ? <Route path="*" element={<Auth/>}/> : <Route path="*" element={<Chats/>}/>
            }
        </Routes>);
})

export default AppRouter