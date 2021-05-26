import React, { Component, useState } from "react";
import {Link} from "react-router-dom";
import '../../css/Login.css';

async function loginUser(credentials) {
    return fetch('https://wandelapp.azurewebsites.net/api/User/authenticate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=UTF-8'
            },
            body: JSON.stringify(credentials)
        })
        .then((response) => {
            return response.json();
        })
        .catch((error) => {
            console.log(error);
        })
}

export default function Login({ setToken }) {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [error, setError] = useState();

    const handleSubmit = async event => {
        event.preventDefault();
        var result = await loginUser({'email': email, 'password': password})
        if (result != 'Problem with loggin in. Check provided credentials.') {
            setToken(result);
        } else {
            setError(result);
        }
    }

    return (
        <div className="loginDiv">
            {
                error && 
                <div className="error">
                    <p className="errorText">{error}</p>
                    <button onClick={() => setError(null)} className="closeError">X</button>
                </div>
            }
            <h1>WandelApp</h1>
            <form onSubmit={handleSubmit}>
                <label>
                    <p>Email</p>
                    <input type="text" onChange={e => setEmail(e.target.value)} />
                </label>
                <label>
                    <p>Password</p>
                    <input type="password" onChange={e => setPassword(e.target.value)} />
                </label>
                <div id="btnDiv">
                    <button type="submit" id="btnLogin">Login</button>
                </div>
            </form>
            <Link to="/Register">
                <p>Sign Up</p>
            </Link>
        </div>
    )
}