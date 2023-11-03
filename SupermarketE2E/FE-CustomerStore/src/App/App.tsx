import React from "react";
import { Route, Routes } from "react-router-dom";
import Categories from "../Categories/Categories";
import Navbar, { NavBarLink } from "../Components/Navbar";
import Home from "../Pages/Home";
import Footer from "./Footer";

var topNav: NavBarLink[] = [
  { url: '/', display: 'Home' },
  { url: '/login', display: 'Login' },
];

var middleNav: NavBarLink[] = [
  { url: '/', display: 'Store' }
];

function App() {
  return (
    <div className="container mx-auto">
      <nav className="w-full">
        <Navbar links={topNav} right={true}/>
      </nav>
      <div className="w-full">
        <img src="https://via.placeholder.com/600x200" className="w-full h-64"/>
      </div>
      <div className="w-full mb-2">
        <Navbar links={middleNav}/>
      </div>
      <Routes>
        <Route path="/" element={ <Home /> } />
      </Routes>
      <div className="w-full">
        <Footer />
      </div>
    </div>
  );
}

export default App;