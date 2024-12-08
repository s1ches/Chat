import routesNames from "./routesNames.js";
import Chats from "../../pages/chats/Chats.jsx";
import Auth from "../../pages/auth/Auth.jsx";

export const notAuthRoutes = [
    {
        path: routesNames.AUTH,
        Component: Auth
    }
]

export const authRoutes = [
    {
        path: routesNames.CHATS,
        Component: Chats,
    }
]