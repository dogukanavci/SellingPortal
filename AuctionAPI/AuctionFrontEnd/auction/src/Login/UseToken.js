import { useState } from 'react';

export default function UseToken() {
  const getToken = () => {
    const tokenString = sessionStorage.getItem('token');
    return tokenString
  };

  const [token, setToken] = useState(getToken());

  const saveToken = userToken => {
    sessionStorage.setItem('token', JSON.stringify(userToken.token));
    sessionStorage.setItem('bidderId', JSON.stringify(userToken.bidderId));
    setToken(userToken.token);
  };

  return {
    setToken: saveToken,
    token
  }
}