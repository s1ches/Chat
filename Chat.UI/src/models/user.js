import {makeAutoObservable} from "mobx";

export default class User {
    _id;
    _role;
    _username;

    constructor() {
        this._id = ''
        this._role = "User"
        this._username = ""
        makeAutoObservable(this)
    }

    static init(id, userName, role) {
        let newUser = new User()

        newUser._id = id;
        newUser._username = userName;
        newUser._role = role;

        return newUser
    }

    get role() {
        return this._role
    }

    get id() {
        return this._id;
    }

    get username() {
        return this._username
    }
}