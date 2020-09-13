import React from 'react';
import logo from './logo.svg';
import './App.css';

import * as Components from './Components/index'

function App() {
  return (
    <div className="App">
      <Components.FirstComponent name="Spatiq" />
      <Components.SecondComponent/>
    </div>
  );
}


export default App;
