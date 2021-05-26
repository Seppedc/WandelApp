import React, {useCallback, useMemo, useState, useEffect} from "react";
import {
    MapContainer,
    TileLayer
} from 'react-leaflet'
import L from 'leaflet';

import character from '../Images/test2.png';
import SimonSays from '../Images/SimonSays.jpg';
import {useHistory } from 'react-router-dom';
let popupError = false;
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
    iconRetinaUrl:null,
    iconUrl:character,
    shadowUrl:null
})
var markerIcons = {
    "User": L.icon({
        iconRetinaUrl:null,
        iconUrl: character,
        iconSize: [30, 50],
        shadowUrl:null,
        draggable:false
    }),
    "SimonSays": L.icon({
        iconUrl: SimonSays,
        iconSize: [20, 20],
        shadowUrl:null,
        draggable:false
    }),
}

let center = [51.041, 3.758];
const zoom = 18;
let marker = new L.Marker(center, {icon: markerIcons["User"]});
let gameLocations = [];
let markers = [];
function DisplayPosition({ map }) {
    let history = useHistory();
    map.locate({watch: true})
        .on('locationfound', function(e){
            center = [e.latitude, e.longitude];
            onClick();

            for (let y =0; y<markers.length;y++){
                map.removeLayer(markers[y]);
            }
            markers = [];
            marker = new L.Marker(e.latlng, {icon: markerIcons["User"]});
            markers.push(marker);
            for (let i=0; i<gameLocations.length;i++) {
                let game = new L.Marker(gameLocations[i], {icon: markerIcons["SimonSays"]}).on('click', function () {
                    for (let y =0; y<markers.length;y++){
                        map.removeLayer(markers[y]);
                    }
                    history.push('/SimonSays');
                });
                markers.push(game);
            }

            for (let y =0; y<markers.length;y++){
                map.addLayer(markers[y]);
            }
            map.dragging.disable();
            map.touchZoom.disable();
            map.doubleClickZoom.disable();
            map.scrollWheelZoom.disable();
            map.boxZoom.disable();
            map.keyboard.disable();
            if (map.tap) map.tap.disable();
        })
        .on('locationerror', function(e){
            if(!popupError){
                console.log(e);
                alert("Location access denied.");
            }
            popupError = true;
        });
    const [position, setPosition] = useState(map.getCenter());

    const onClick = useCallback(() => {
        map.setView(center, zoom);
    }, [map])

    const onMove = useCallback(() => {
        setPosition(map.getCenter())
    }, [map]);

    return (
       null
    )
}
//get games

function InitializeGames() {
    gameLocations = [];
    let amount = randomInteger(3,6);
    for (let i=0; i<amount;i++){
        let yAdjust = randomInteger(0,30) -15;
        let xAdjust = randomInteger(0,30)-15;
        gameLocations.push([center[0]+(yAdjust/10000),center[1]+(xAdjust/10000)])
    }
}
function randomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}


function Map()  {
    const [map, setMap] = useState(null);
    var MINUTE_MS = 100;//elke 10sec atm

    useEffect(() => {
        const interval = setInterval(() => {
            InitializeGames();
            MINUTE_MS = 10000;
        }, MINUTE_MS );
        return () => clearInterval(interval);
    }, []);

    const displayMap = useMemo(
        () => (
            <MapContainer
                center={center}
                zoom={zoom}
                scrollWheelZoom={false}
                whenCreated={setMap}>
                <TileLayer
                    attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
            </MapContainer>
        ),
        [],
    )
    return (
        <div>
            {map ? <DisplayPosition map={map} /> : null}
            {displayMap}
        </div>
    )
}
export default Map;

