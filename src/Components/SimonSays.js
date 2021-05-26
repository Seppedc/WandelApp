import React from 'react';
import imageReturn from "../Images/Return.png";
import '../css/AR.css';
import '../css/SimonSays.css';
import audioRood from '../Audio/red.mp3';
import audioGroen from '../Audio/green.mp3';
import audioBlauw from '../Audio/blue.mp3';
import audioGeel from '../Audio/yellow.mp3';
import { useRef, useEffect,useState } from 'react';

let volgorde =[];
let userVolgorde =[];
let totaalHoeveelheid = 4;
const rood = new Audio(audioRood);
const groen = new Audio(audioGroen);
const blauw = new Audio(audioBlauw);
const geel = new Audio(audioGeel);
let audioPointer = 0;
let audioArray = [];
let disableUserInput= true;
function SimonSays () {
    const [activateAR,SetActivityAR] = useState(false);

    async function EndGame() {
        let element2 = document.getElementById("Winnaar");
        element2.classList.remove("hidden");
        await sendToBackend();
        await timeout(2000);
        window.location.href="/";
    }
    async function sendToBackend(){
        let loggedInUser = JSON.parse(sessionStorage.getItem('token'));
        let gameId = await getGameId("Simon Says");
        let resultUserGame = fetch('https://wandelapp.azurewebsites.net/api/UserGame', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=UTF-8',
                'Authorization': 'Bearer ' + loggedInUser.JwtToken,
            },
            body: JSON.stringify({"UserId": loggedInUser.Id, "GameId": gameId})
        })
        .then((response) => {
            return response.json();
        })
        .catch((error) => {
            console.log(error);
        });
    }
    async function getGameId(gameName) {
        let token = JSON.parse(sessionStorage.getItem('token')).JwtToken;
        return fetch('https://wandelapp.azurewebsites.net/api/Game/getIdByName/' + gameName, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer ' + token,
            }
        })
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                return data;
            })
            .catch((error) => {
                console.log(error);
            })
    }
    function switchAR() {
        window.ar();
        SetActivityAR(!activateAR);
        console.log(activateAR);
    }

    function Startup() {
        Reset();
        GenerateColor(totaalHoeveelheid);
        let element1 = document.getElementById("StartGame");
        element1.classList.add("hidden");
    }
    function GenerateColor(aantal){
        disableUserInput=true;
        if(volgorde.length ===totaalHoeveelheid){
            EndGame();
        }else{
            let generated = Math.floor(Math.random() * aantal)+1;
            AddColor(generated);
        }
    }
    function AddColor(nummer){
        volgorde.push(nummer);
        console.log(volgorde);
        AddSound(nummer);
        PlayGenerated();
    }
    function AddSound(nummer) {
        if(nummer===1){
            audioArray.push(audioRood)
        }else if(nummer===2){
            audioArray.push(audioGroen)
        }else if(nummer===3){
            audioArray.push(audioBlauw)
        }else if(nummer===4){
            audioArray.push(audioGeel)
        }
    }
    async function PlayGenerated() {
        ClearShowColor();
        await timeout(500);
        if (audioPointer < audioArray.length) {
            let audio = new Audio(audioArray[audioPointer]);
            audio.addEventListener("ended", PlayGenerated);
            ShowColor(audioArray[audioPointer]);
            audioPointer += 1;
            audio.play().then(r => PlayGenerated);
        } else {
            audioPointer=0;
            ClearShowColor();
            disableUserInput=false;
        }
    }
    function ShowColor(color) {
         if(color===audioRood){
             let red = document.querySelector('#red');
             red.setAttribute('color','#ffffff' );

         }else if(color===audioGroen){
             let green = document.querySelector('#green');
             green.setAttribute('color','#ffffff' );
         }else if(color===audioBlauw){
             let blue = document.querySelector('#blue');
             blue.setAttribute('color','#ffffff' );
         }else if(color===audioGeel){
             let yellow = document.querySelector('#yellow');
             yellow.setAttribute('color','#ffffff' );
         }
    }
    function ClearShowColor() {
        let red = document.querySelector('#red');
        red.setAttribute('color','red' );
        let green = document.querySelector('#green');
        green.setAttribute('color','green' );
        let blue = document.querySelector('#blue');
        blue.setAttribute('color','blue' );
        let yellow = document.querySelector('#yellow');
        yellow.setAttribute('color','yellow' );
    }


//user
    async function UserAddColor(nummer) {
        if(!disableUserInput){
            ClearShowColor();
            UserPlaySound(nummer);
            userVolgorde.push(nummer);
            await timeout(1000);
            ClearShowColor();
            Controleer();
        }
    }
    function UserPlaySound(nummer){
         if(nummer===1){
             let red = document.querySelector('#red');
             red.setAttribute('color','#ffffff' );
             rood.play().then(r => ClearShowColor);
         }else if(nummer===2){
             let green = document.querySelector('#green');
             green.setAttribute('color','#ffffff' );
             groen.play().then(r => ClearShowColor);
         }else if(nummer===3){
             let blue = document.querySelector('#blue');
             blue.setAttribute('color','#ffffff' );
             blauw.play().then(r => ClearShowColor);
         }else if(nummer===4){
             let yellow = document.querySelector('#yellow');
             yellow.setAttribute('color','#ffffff' );
             geel.play().then(r => ClearShowColor);
         }
    }
    function Controleer() {
        if(volgorde.length === userVolgorde.length){
            let failed = false;
            for (let i = 0; i < volgorde.length; i++){
                if(volgorde[i]!==userVolgorde[i]){
                    UserFail();
                    failed = true;
                }
            }
            if(!failed){
                GenerateColor(totaalHoeveelheid);
                userVolgorde=[];
            }
        }else{
            for (let i = 0; i < userVolgorde.length; i++){
                if(userVolgorde[i]!==volgorde[i]){
                    UserFail();
                }
            }
        }
    }
    async function UserFail() {
        let element2 = document.getElementById("Fail");
        element2.classList.remove("hidden");
        await timeout(1500);
        element2.classList.add("hidden");
        Startup();
    }

    function Reset() {
        volgorde =[];
        userVolgorde=[];
        audioPointer = 0;
        audioArray = [];
    }
    function timeout(delay) {
        return new Promise( res => setTimeout(res, delay) );
    }
    function GoBack() {
        window.location.href="/";
    }
    let object;
    if(activateAR) {
        object="";
    } else {
        object = <a-entity environment="preset: forest; playArea: 10; shadow: false; groundTexture: walkernoise; groundColor: #68bf4d;groundColor2: #2f8611; skyType:gradient;"></a-entity>;
    }
    return (
        <div >
            <img id="ButtonBack" onClick={function(e) {GoBack();}} src={imageReturn} alt="Go Back" width='35px' height='25px'/>
            <button id="ButtonAR" onClick={function(e) {switchAR();}} alt="AR" width='35px' height='25px'>AR</button>
            <div className='fullscreenDiv'>
                <label className="center" id="StartGame" onClick={function(e) {Startup();}}>Start</label>
                <label className="center hidden" id="Winnaar" >Proficiat</label>
                <label className="center hidden" id="Fail" >Fail</label>
            </div>
            <video id="webcam" autoPlay playsinline></video>
            <a-scene cursor="rayOrigin: mouse" vr-mode-ui="enabled: false"  >
                {object}
                <a-entity id="rig" position="0 0 0" rotation="0 0 0">
                    <a-camera  id="camera" reverse-mouse-drag="true" wasd-controls-enabled="false"></a-camera>
                </a-entity>
                <a-box id="red" position='-0.25 2 -2' scale='0.35 0.35 0.35' material='color: red'  onClick={function(e) {UserAddColor(1);}}></a-box>
                <a-box id="green" position='0.25 2 -2' scale='0.35 0.35 0.35' material='color: green'  onClick={function(e) {UserAddColor(2);}}></a-box>
                <a-box id="blue" position='-0.25 1.5 -2' scale='0.35 0.35 0.35' material='color: blue'  onClick={function(e) {UserAddColor(3);}}></a-box>
                <a-box id="yellow" position='0.25 1.5 -2' scale='0.35 0.35 0.35' material='color: yellow'  onClick={function(e) {UserAddColor(4);}}></a-box>
                <a-entity light="type: directional; color: #EEE; intensity: 0.5" position="0 0 1"></a-entity>

            </a-scene>

        </div>
    )
}

export default SimonSays;