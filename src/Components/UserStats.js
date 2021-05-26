import React, { useState, useEffect } from 'react';
import '../css/UserStats.css';

async function getGameStats(userGames, JwtToken) {
    return fetch('https://wandelapp.azurewebsites.net/api/UserGame/' + userGames, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + JwtToken,
        }
    })
    .then((response) => {
        return response.json();
    })
    .catch((error) => {
        console.log("ERROR: " + error);
    })
}

export default function UserStats (props) {
    const { userGames, JwtToken } = props;
    const [gameStatistics, setGameStatistics] = useState(false);

    useEffect(async () => {
        await getGameStats(userGames[0], JwtToken).then((result) => setGameStatistics(result));
    }, []);

    if (!gameStatistics) {
        return (
            <div style={{ display: 'flex', flex: 1, justifyContent: 'center', alignItems: 'center' }}>
                <h3>Getting user statistics...</h3>
            </div>
        )
    } else {
        return (
            <div className="gameStats">
                <h3>{gameStatistics.GameName}</h3>
                <p className="played">Played: {gameStatistics.CountPlayed} times</p>
                <p className="earned">XP earned: {gameStatistics.TotalPointsEarnedWithGame} xp</p>
            </div>
        )
    }
}