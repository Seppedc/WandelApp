import React from 'react';
import '../css/MainPage.css';
import Map from "./Map";
import {Link} from "react-router-dom";

function MainPage () {
    return (
        <div id="mainPageContainer">
            <div id="MapDiv">
                <div id="MapId">
                    <Link to="/Profile">
                        <button id="ButtonProfile">
                            Profile
                        </button>
                    </Link>
                </div>
                <Map id="Map"/>
            </div>
        </div>
    )
}
export default MainPage;


