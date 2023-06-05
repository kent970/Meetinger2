import React from 'react';
import './App.css';
import Header from './Components/Header';

function App() {
    return (
    <div className="container">
            

        <h1 className="title">Welcome to the Meeting Scheduling System!</h1>
            <p className="description">Let's schedule and manage your meetings efficiently.</p>
<Header /> 
        </div>
);
}

export default App;