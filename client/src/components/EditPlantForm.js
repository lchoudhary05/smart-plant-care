import React, { useState, useEffect } from "react";
import { updatePlant } from "../api";
import "./Auth.css";

const EditPlantForm = ({ plant, onPlantUpdated, onCancel }) => {
  const [formData, setFormData] = useState({
    name: "",
    species: "",
    wateringFrequencyDays: 7,
    sunlight: "",
    location: "",
    notes: "",
    fertilizingFrequencyDays: 30,
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  // Initialize form with plant data when component mounts or plant changes
  useEffect(() => {
    if (plant) {
      setFormData({
        name: plant.name || "",
        species: plant.species || "",
        wateringFrequencyDays: plant.wateringFrequencyDays || 7,
        sunlight: plant.sunlight || "",
        location: plant.location || "",
        notes: plant.notes || "",
        fertilizingFrequencyDays: plant.fertilizingFrequencyDays || 30,
      });
    }
  }, [plant]);

  const handleChange = (e) => {
    const { name, value, type } = e.target;
    setFormData({
      ...formData,
      [name]: type === "number" ? parseInt(value) : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
        await updatePlant(plant.id, formData);
      
      // Notify parent component
      if (onPlantUpdated) onPlantUpdated();
    } catch (error) {
      setError(error.message || "Failed to update plant");
    } finally {
      setIsLoading(false);
    }
  };

  if (!plant) {
    return null;
  }

  return (
    <div className="edit-plant-modal">
      <div className="edit-plant-content">
        <div className="edit-plant-header">
          <h3>✏️ Edit Plant: {plant.name}</h3>
          <button 
            type="button" 
            className="close-button"
            onClick={onCancel}
          >
            ×
          </button>
        </div>
        
        {error && <div className="error-message">{error}</div>}
        
        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="edit-name">Plant Name *</label>
              <input
                type="text"
                id="edit-name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
                placeholder="e.g., Monstera Deliciosa"
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="edit-species">Species</label>
              <input
                type="text"
                id="edit-species"
                name="species"
                value={formData.species}
                onChange={handleChange}
                placeholder="e.g., Monstera"
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="edit-wateringFrequencyDays">Water Every (days) *</label>
              <input
                type="number"
                id="edit-wateringFrequencyDays"
                name="wateringFrequencyDays"
                value={formData.wateringFrequencyDays}
                onChange={handleChange}
                min="1"
                max="30"
                required
              />
            </div>
            
            <div className="form-group">
              <label htmlFor="edit-fertilizingFrequencyDays">Fertilize Every (days)</label>
              <input
                type="number"
                id="edit-fertilizingFrequencyDays"
                name="fertilizingFrequencyDays"
                value={formData.fertilizingFrequencyDays}
                onChange={handleChange}
                min="7"
                max="90"
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="edit-sunlight">Sunlight Requirements</label>
              <select
                id="edit-sunlight"
                name="sunlight"
                value={formData.sunlight}
                onChange={handleChange}
              >
                <option value="">Select sunlight preference</option>
                <option value="Full sun">Full sun</option>
                <option value="Bright indirect">Bright indirect</option>
                <option value="Medium light">Medium light</option>
                <option value="Low light">Low light</option>
                <option value="Shade">Shade</option>
              </select>
            </div>
            
            <div className="form-group">
              <label htmlFor="edit-location">Location</label>
              <input
                type="text"
                id="edit-location"
                name="location"
                value={formData.location}
                onChange={handleChange}
                placeholder="e.g., Living room window"
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="edit-notes">Care Notes</label>
            <textarea
              id="edit-notes"
              name="notes"
              value={formData.notes}
              onChange={handleChange}
              placeholder="Any special care instructions or notes..."
              rows="3"
            />
          </div>

          <div className="edit-plant-actions">
            <button 
              type="button" 
              className="auth-button secondary"
              onClick={onCancel}
              disabled={isLoading}
            >
              Cancel
            </button>
            <button 
              type="submit" 
              className="auth-button primary"
              disabled={isLoading}
            >
              {isLoading ? "Updating..." : "Update Plant"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditPlantForm; 