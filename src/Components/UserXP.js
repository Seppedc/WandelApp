import React from 'react';

export default function UserXP (props) {
    const { currentXP, neededXP, nextLevelName } = props;

    const container = {
        height: 20,
        width: '85%',
        backgroundColor: "#BEBEBE",
        borderRadius: 50,
        margin: "0 auto 4rem"
    }
    const progressbar = {
        height: '100%',
        width: `${(currentXP / neededXP) * 100}%`,
        backgroundColor: "red",
        borderRadius: 'inherit',
        textAlign: 'right'
    }
    const progress = {
        padding: 5,
        color: 'white',
        fontWeight: 'bold'
    }
    const next = {
        margin: '0.5rem 0 0 0'
    }

    return (
        <div style={container}>
            <div style={progressbar}>
                <span style={progress}>{currentXP}xp</span>
            </div>
            <p style={next}>{neededXP}xp needed for <br></br>"{nextLevelName}"</p>
        </div>
    )
}