import {makeAutoObservable} from "mobx";
import {Role} from "../models/enums/role.js";
import User from "../models/user.js";

export default class UserStore {
    _user = new User()
    _chats = [];
    _isAuth = false
    _connection = null

    constructor() {
        this._user = new User();
        this._chats = []
        makeAutoObservable(this)
    }

    login(user) {
        this._isAuth = true
        this._user = user
        this._isAdmin = user.role === Role.admin
    }

    logout() {
        this._isAuth = false
        this._user = new User()
    }

    get isAuth() {
        return this._isAuth
    }

    get chats(){
        return this._chats
    }

    set chats(chats) {
        this._chats = chats
    }

    get connection(){
        return this._connection
    }

    set connection(connection) {
        this._connection = connection
    }

    get isAdmin() {
        return this._isAdmin
    }

    get user() {
        return this._user;
    }
}