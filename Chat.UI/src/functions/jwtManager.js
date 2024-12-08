import User from "../models/user.js";

export function getUserInfo(jwt) {
    const tokenParts = jwt.split('.'); // Разделяем токен на части
    const decodedPayload = JSON.parse(atob(tokenParts[1])); // Декодируем полезную нагрузку
    const id = decodedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    const name = decodedPayload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    const role = decodedPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
    return User.init(id, name, role);
}

export function isExpired(jwt) {
    const tokenParts = jwt.split('.'); // Разделяем токен на части
    const decodedPayload = JSON.parse(atob(tokenParts[1])); // Декодируем полезную нагрузку
    const expiryTime = decodedPayload["exp"]; // Время истечения в формате UNIX

    // Получаем текущее время в секундах
    const currentTime = Math.floor(Date.now() / 1000);

    // Сравниваем текущий момент с временем истечения
    return expiryTime < currentTime;
}

export function needUpdate(jwt) {
    const tokenParts = jwt.split('.'); // Разделяем токен на части
    const decodedPayload = JSON.parse(atob(tokenParts[1])); // Декодируем полезную нагрузку
    const expiryTime = decodedPayload["exp"]; // Время истечения в формате UNIX

    // Сравниваем текущий момент с временем истечения
    return expiryTime - Math.floor(Date.now() / 1000) < 5 * 60;
}
