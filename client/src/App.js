import React, { useState, useEffect } from "react";
import { isAuthenticated } from "./api";
import Header from "./components/Header";
import Login from "./components/Login";
import Register from "./components/Register";
import PlantForm from "./components/PlantForm";
import PlantList from "./components/PlantList";
import "./App.css";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [showRegister, setShowRegister] = useState(false);
  const [user, setUser] = useState(null);
  const [refresh, setRefresh] = useState(false);

  // Check if user is already logged in on app start
  useEffect(() => {
    const checkAuth = () => {
      if (isAuthenticated()) {
        const storedUser = localStorage.getItem("user");
        if (storedUser) {
          setUser(JSON.parse(storedUser));
          setIsLoggedIn(true);
        }
      }
    };

    checkAuth();
  }, []);

  const handleLoginSuccess = (userData) => {
    setUser(userData);
    setIsLoggedIn(true);
    setShowRegister(false);
  };

  const handleRegisterSuccess = (userData) => {
    setUser(userData);
    setIsLoggedIn(true);
    setShowRegister(false);
  };

  const handleLogout = () => {
    setUser(null);
    setIsLoggedIn(false);
    setRefresh(!refresh);
  };

  const handlePlantAdded = () => {
    setRefresh(!refresh);
  };

  // Show authentication forms if not logged in
  if (!isLoggedIn) {
    return (
      <div className="App">
        {showRegister ? (
          <Register 
            onRegisterSuccess={handleRegisterSuccess}
            onSwitchToLogin={() => setShowRegister(false)}
          />
        ) : (
          <Login 
            onLoginSuccess={handleLoginSuccess}
            onSwitchToRegister={() => setShowRegister(true)}
          />
        )}
      </div>
    );
  }

  // Show main app if logged in
  return (
    <div className="App">
      <Header user={user} onLogout={handleLogout} />
      
      <main className="main-content">
        <PlantForm onPlantAdded={handlePlantAdded} />
        <PlantList refresh={refresh} />
      </main>
    </div>
  );
}

export default App;
