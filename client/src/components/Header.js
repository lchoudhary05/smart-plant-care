import React from "react";
import { logout } from "../api";
import "./Header.css";

const Header = ({ user, onLogout }) => {
  const handleLogout = () => {
    logout();
    onLogout();
  };

  return (
    <header className="header">
      <div className="header-content">
        <div className="header-left">
          <h1 className="header-title">🌿 Simran baby app</h1>
        </div>

        {user && (
          <div className="header-right">
            <div className="user-info">
              <span className="user-name">
                Welcome, {user.firstName} {user.lastName}
              </span>
              <span className="user-tier">
                {user.subscriptionTier} Plan • {user.maxPlants} plants max
              </span>
            </div>
            <button onClick={handleLogout} className="logout-button">
              Logout
            </button>
          </div>
        )}
      </div>
    </header>
  );
};

export default Header;
