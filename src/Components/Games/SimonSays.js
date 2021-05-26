import React, { Component,useState } from 'react';

import '../../css/SimonSays.css';
import audioRood from '../../Audio/red.mp3';
import audioGroen from '../../Audio/green.mp3';
import audioBlauw from '../../Audio/blue.mp3';
import audioGeel from '../../Audio/yellow.mp3';
import imageReturn from '../../Images/Return.png';
import {useHistory} from 'react-router-dom';
//kleuren volggorde :
//1 = rood  (links boven)
//2 = groen (rechts boven)
//3 = blauw (rechts onder)
//4 = geel  (links onder)

let volgorde =[];
let userVolgorde =[];
let totaalHoeveelheid = 4;
let showButton = '';
const rood = new Audio(audioRood);
const groen = new Audio(audioGroen);
const blauw = new Audio(audioBlauw);
const geel = new Audio(audioGeel);
let audioPointer = 0;
let audioArray = [];
let disableUserInput= true;
class SimonSays extends Component {
/*
    render() {
        return (
            <div id="SimonSays">
                <Link id="divReturn" to="/">
                    <img id="Return" src={imageReturn} alt="Go Back" width='35px' height='25px'/>
                </Link>
                <div id="Colors">
                    <div id="Rood"  onClick={function(e) {UserAddColor(1);}}></div>
                    <div id="Groen" onClick={function(e) {UserAddColor(2);}}></div>
                    <div id="Geel"  onClick={function(e) {UserAddColor(4);}}></div>
                    <div id="Blauw" onClick={function(e) {UserAddColor(3);}}></div>
                </div>
                <div>
                    <button  id="Start" className={showButton} onClick={function(e) {Startup();}}>Start</button>
                </div>
            </div>
        );
    }

 */
}
function SimonSays2() {
    let history = useHistory();
    async function EndGame() {
        console.log("win");
        await timeout(2000);
        window.location.href="/";
    }
    function Startup() {
        Reset();
        GenerateColor(totaalHoeveelheid);
        let element1 = document.getElementById("Start");
        element1.classList.add("hidden");}
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
            let element1 = document.getElementById("Rood");
            element1.classList.add("Animate");
        }else if(color===audioGroen){
            let element2 = document.getElementById("Groen");
            element2.classList.add("Animate");
        }else if(color===audioBlauw){
            let element3 = document.getElementById("Blauw");
            element3.classList.add("Animate");
        }else if(color===audioGeel){
            let element4 = document.getElementById("Geel");
            element4.classList.add("Animate");
        }
    }
    function ClearShowColor() {
        let element1 = document.getElementById("Rood");
        element1.classList.remove("Animate");
        let element2 = document.getElementById("Groen");
        element2.classList.remove("Animate");
        let element3 = document.getElementById("Blauw");
        element3.classList.remove("Animate");
        let element4 = document.getElementById("Geel");
        element4.classList.remove("Animate");
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
            let element1 = document.getElementById("Rood");
            element1.classList.add("Animate");
            rood.play().then(r => ClearShowColor);
        }else if(nummer===2){
            let element2 = document.getElementById("Groen");
            element2.classList.add("Animate");
            groen.play().then(r => ClearShowColor);
        }else if(nummer===3){
            let element3 = document.getElementById("Blauw");
            element3.classList.add("Animate");
            blauw.play().then(r => ClearShowColor);
        }else if(nummer===4){
            let element4 = document.getElementById("Geel");
            element4.classList.add("Animate");
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
    function UserFail() {
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
    return (
        <div id="SimonSays">
            <img id="Return" onClick={function(e) {GoBack();}} src={imageReturn} alt="Go Back" width='35px' height='25px'/>
            <div id="Colors">
                <div id="Rood"  onClick={function(e) {UserAddColor(1);}}></div>
                <div id="Groen" onClick={function(e) {UserAddColor(2);}}></div>
                <div id="Geel"  onClick={function(e) {UserAddColor(4);}}></div>
                <div id="Blauw" onClick={function(e) {UserAddColor(3);}}></div>
            </div>
            <div>
                <button  id="Start" className={showButton} onClick={function(e) {Startup();}}>Start</button>
            </div>
        </div>
    );
}

export default SimonSays2;