import React from "react";
import {BrowserRouter as Router,Switch,Route } from 'react-router-dom';
import './App.css';
import useToken from './Components/Authentication/useToken';
import NotFoundOrLoggedIn from './Components/NotFoundOrLoggedIn';
import LoginPage from "./Components/Authentication/Login";
import RegisterPage from "./Components/Authentication/Register";
import MainPage from "./Components/MainPage";
import Profile from "./Components/Profile";
import SimonSaysOld from "./Components/Games/SimonSays";
import SimonSays from "./Components/SimonSays";

function App() {
  const { token, setToken } = useToken();

  if (!token) {
    return (
      <Router>
        <Switch>
          <Route path="/" exact component={(routeProps)=><LoginPage {...routeProps} setToken={setToken}/> }/>
          <Route path="/Register" exact component={(routeProps)=><RegisterPage {...routeProps} setToken={setToken}/> }/>
          <Route component={(routeProps)=><NotFoundOrLoggedIn {...routeProps} 
            text="You're not logged in, please login first." backBtnText="Back to login"/>} />
        </Switch>
      </Router>
    )
  }

  return (
      <Router>
        <Switch>
            <Route path="/" exact component={MainPage}/>
            <Route path="/Profile" exact component={Profile}/>
            <Route path="/SimonSaysOld" exact component={SimonSaysOld}/>
            <Route path="/SimonSays" exact component={SimonSays}/>
            <Route component={(routeProps)=><NotFoundOrLoggedIn {...routeProps} 
              text="Page not found." backBtnText="Back to Home"/>} />
        </Switch>
      </Router>
  );
}

export default App;
