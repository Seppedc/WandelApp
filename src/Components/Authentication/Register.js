import React, { useState } from "react";
import {Link} from "react-router-dom";
import '../../css/Registration.css';
import imageReturn from "../../Images/Return.png";

async function registerUser(credentials) {
    return fetch('https://wandelapp.azurewebsites.net/api/User', {
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

export default function Register ({ setToken }) {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [repeatPassword, setRepeatPassword] = useState();
    const [username, setUsername] = useState();
    const [age, setAge] = useState();
    const [error, setError] = useState();

    const handleSubmit = async event => {
        event.preventDefault();
        var result = await registerUser({'email': email, 'password': password, 'repeatPassword': repeatPassword, 'username': username, 'age': parseInt(age)})
        if (!result.hasOwnProperty('JwtToken')) {
            setError('Error... Please check if the form is filled in correctly and try again.');
        } else {
            setToken(result);
        }
    }

    return (
        <div id="registrationDiv">
            <Link id="divReturn" to="/">
                <img id="Return" src={imageReturn} alt="Go Back" width='35px' height='25px'/>
            </Link>
            {
                error && 
                <div className="error">
                    <p className="errorText">{error}</p>
                    <button onClick={() => setError(null)} className="closeError">X</button>
                </div>
            }
            <h1>Registration</h1>
            <form onSubmit={handleSubmit}>
                <label>
                    <p>Username</p>
                    <input type="text" onChange={e => setUsername(e.target.value)} />
                </label>
                <label>
                    <p>Age</p>
                    <input type="text" onChange={e => setAge(e.target.value)} />
                </label>
                <label>
                    <p>Email</p>
                    <input type="text" onChange={e => setEmail(e.target.value)} />
                </label>
                <label>
                    <p>Password</p>
                    <input type="password" onChange={e => setPassword(e.target.value)} />
                </label>
                <label>
                    <p>Repeat password</p>
                    <input type="password" onChange={e => setRepeatPassword(e.target.value)} />
                </label>
                <div id="btnDiv">
                    <button type="submit" id="btnSignUp">Sign up</button>
                </div>
            </form>
        </div>
    )
}