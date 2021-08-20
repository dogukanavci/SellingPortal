import React, { useState } from 'react';
import PropTypes from 'prop-types';

import './Login.scss';

var users = {"Mike" : "123","Joe": "456"};
var bidderIds = {"Mike" : 2,"Joe": 1};

function loginUser(credentials) {
  if(credentials.username in users && users[credentials.username] === credentials.password){
    return {token: "token", bidderId: bidderIds[credentials.username] };
  } 
  return {token: null, bidderId: null };
 }

export default function Login({ setToken }) {
  const [username, setUserName] = useState();
  const [password, setPassword] = useState();

  const handleSubmit = async e => {
    e.preventDefault();
    const response = loginUser({
      username,
      password
    });
    setToken(response);
  }

  return(
    <div className="login-wrapper">
      <form onSubmit={handleSubmit}>
        <h3 className="mt-5">Login</h3>
        <label>
          <p className="mx-5 mb-1">Username</p>
          <input className="mx-5" type="text" onChange={e => setUserName(e.target.value)}/>
        </label>
        <label>
          <p className="mb-1">Password</p>
          <input type="password" onChange={e => setPassword(e.target.value)}/>
        </label>
        <div className="d-flex justify-content-center mt-3">
          <button type="submit">Login</button>
        </div>
      </form>
    </div>
  )
}

Login.propTypes = {
  setToken: PropTypes.func.isRequired
};