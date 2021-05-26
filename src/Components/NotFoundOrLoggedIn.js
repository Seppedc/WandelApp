import React from "react";
import {Link} from "react-router-dom";

export default function NotFoundOrLoggedIn(props) {
    const { text, backBtnText } = props;
    return (
        <div style={{ textAlign: 'center' }}>
            <h1>{text}</h1>
            <Link id="logoutLink" to="/">
                {backBtnText}
            </Link>
        </div>
    )
}