import React, { useState, useEffect } from 'react';
import {Link} from "react-router-dom";
import '../css/Profile.css';
import player_img from '../img/player.png';
import UserStats from './UserStats';
import UserXP from './UserXP';
import imageReturn from "../Images/Return.png";

function GoBack() {
    window.location.href="/";
}

async function getUser(userToken) {
    return fetch('https://wandelapp.azurewebsites.net/api/User/' + userToken.Id, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + userToken.JwtToken,
            }
        })
        .then((response) => {
            return response.json();
        })
        .catch((error) => {
            console.log(error);
        })
}

export default function Profile () {
    const [user, setUser] = useState();
    useEffect(async () => {
        const tokenString = sessionStorage.getItem('token');
        if (!tokenString) { 
            window.location.href = "/"; 
        } else {
            const userToken = JSON.parse(tokenString);
            const result = await getUser(userToken);
            result.JwtToken = userToken.JwtToken;
            setUser(result);
        }
    }, []);

    const handleLogout = (event) => {
        event.preventDefault();
        sessionStorage.clear();
        window.location.href = '/';
    }

    let gameStats;
    if (user && user.UserGames.length > 0) {
        gameStats = <UserStats userGames={user.UserGames} JwtToken={user.JwtToken}/>
    } else {
        gameStats = <p>No user statistics yet...</p>
    }

    if (user == null) {
        return (
            <div id="profileContainer" style={{ display: 'flex', flex: 1, justifyContent: 'center', alignItems: 'center' }}>
                <h3>Getting user information...</h3>
            </div>
        )
    } else {
        return (
            <div id="profileContainer">
                <img id="Return" onClick={function(e) {GoBack();}} src={imageReturn} alt="Go Back" width='35px' height='25px'/>
                <div id="userInfo">
                    <img src={player_img} alt="profile pic" />
                    <div id="info">
                        <h1 id="userName">{user.UserName}</h1>
                        <p id="userLevel">Level: {user.CurrentLevelName}</p>
                        <UserXP currentXP={user.CurrentPoints} neededXP={user.NextLevelPointsRequired} 
                            nextLevelName={user.NextLevelName}/>
                        <Link id="logoutLink" to="/">
                            <button onClick={handleLogout} id="btnLogout">
                                Logout
                            </button>
                        </Link>
                    </div>
                </div>
                <div id="userStats">
                    <h2>User statistics</h2>
                    {gameStats}
                </div>
            </div>
        )
    }
}