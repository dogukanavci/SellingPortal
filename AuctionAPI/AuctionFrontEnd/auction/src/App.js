import './App.css';
import {Home} from './Home/Home';
import {Item} from './Item/Item';
import {Navigation} from './Navigation';
import Login from './Login/Login';
import UseToken from './Login/UseToken';
import React from 'react';
import {BrowserRouter,Route, Switch} from 'react-router-dom';


function App() {
  const { token, setToken } = UseToken();
  if(!token) {
    return <Login setToken={setToken} />
  }
  return (
    <BrowserRouter>
      <Navigation>
        
      </Navigation>
      <Switch>
          <Route path="/" component={Home} exact/>
          <Route path="/item/:id" component={Item} exact/>
      </Switch>
    </BrowserRouter>
  );
}

export default App;
