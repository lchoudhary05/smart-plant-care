import React, { useEffect, useState } from "react";
import { getPlants } from "../api";
import "./PlantList.css";

const PlantList = ({ refresh }) => {
  const [plants, setPlants] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    async function fetchData() {
      setIsLoading(true);
      setError("");
      
      try {
        const data = await getPlants();
        setPlants(data);
      } catch (error) {
        setError(error.message || "Failed to fetch plants");
        setPlants([]);
      } finally {
        setIsLoading(false);
      }
    }
    
    fetchData();
  }, [refresh]);

  const formatDate = (dateString) => {
    if (!dateString) return "Never";
    const date = new Date(dateString);
    return date.toLocaleDateString();
  };

  const getDaysSinceWatered = (lastWatered) => {
    if (!lastWatered) return "Unknown";
    const days = Math.floor((new Date() - new Date(lastWatered)) / (1000 * 60 * 60 * 24));
    return days;
  };

  const getWateringStatus = (plant) => {
    const daysSinceWatered = getDaysSinceWatered(plant.lastWatered);
    const daysUntilWatering = plant.wateringFrequencyDays - daysSinceWatered;
    
    if (daysUntilWatering <= 0) {
      return { status: "needs-water", text: "Needs water!", className: "status-needs-water" };
    } else if (daysUntilWatering <= 2) {
      return { status: "soon", text: `Water in ${daysUntilWatering} days`, className: "status-soon" };
    } else {
      return { status: "ok", text: `Water in ${daysUntilWatering} days`, className: "status-ok" };
    }
  };

  if (isLoading) {
    return (
      <div className="plant-list-container">
        <h2>🌿 My Plants</h2>
        <div className="loading">Loading your plants...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="plant-list-container">
        <h2>🌿 My Plants</h2>
        <div className="error-message">{error}</div>
      </div>
    );
  }

  if (plants.length === 0) {
    return (
      <div className="plant-list-container">
        <h2>🌿 My Plants</h2>
        <div className="empty-state">
          <p>No plants yet! Add your first plant to get started.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="plant-list-container">
      <h2>🌿 My Plants ({plants.length})</h2>
      
      <div className="plants-grid">
        {plants.map((plant) => {
          const wateringStatus = getWateringStatus(plant);
          
          return (
            <div key={plant._id} className="plant-card">
              <div className="plant-header">
                <h3 className="plant-name">{plant.name}</h3>
                <span className="plant-species">{plant.species}</span>
              </div>
              
              <div className="plant-details">
                <div className="detail-row">
                  <span className="detail-label">Location:</span>
                  <span className="detail-value">{plant.location || "Not specified"}</span>
                </div>
                
                <div className="detail-row">
                  <span className="detail-label">Sunlight:</span>
                  <span className="detail-value">{plant.sunlight || "Not specified"}</span>
                </div>
                
                <div className="detail-row">
                  <span className="detail-label">Water every:</span>
                  <span className="detail-value">{plant.wateringFrequencyDays} days</span>
                </div>
                
                <div className="detail-row">
                  <span className="detail-label">Last watered:</span>
                  <span className="detail-value">
                    {formatDate(plant.lastWatered)} ({getDaysSinceWatered(plant.lastWatered)} days ago)
                  </span>
                </div>
                
                {plant.fertilizingFrequencyDays && (
                  <div className="detail-row">
                    <span className="detail-label">Fertilize every:</span>
                    <span className="detail-value">{plant.fertilizingFrequencyDays} days</span>
                  </div>
                )}
              </div>
              
              <div className={`watering-status ${wateringStatus.className}`}>
                {wateringStatus.text}
              </div>
              
              {plant.notes && (
                <div className="plant-notes">
                  <strong>Notes:</strong> {plant.notes}
                </div>
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default PlantList;
