import React, { useState } from "react";
import { addPlant } from "../api";

const PlantForm = ({ onPlantAdded }) => {
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
      await addPlant(formData);
      
      // Reset form
      setFormData({
        name: "",
        species: "",
        wateringFrequencyDays: 7,
        sunlight: "",
        location: "",
        notes: "",
        fertilizingFrequencyDays: 30,
      });
      
      // Notify parent component
      if (onPlantAdded) onPlantAdded();
    } catch (error) {
      setError(error.message || "Failed to add plant");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="plant-form-container">
      <h3>🌱 Add New Plant</h3>
      
      {error && <div className="error-message">{error}</div>}
      
      <form onSubmit={handleSubmit} className="plant-form">
        <div className="form-row">
          <div className="form-group">
            <label htmlFor="name">Plant Name *</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
              placeholder="e.g., Monstera Deliciosa"
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="species">Species</label>
            <input
              type="text"
              id="species"
              name="species"
              value={formData.species}
              onChange={handleChange}
              placeholder="e.g., Monstera"
            />
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label htmlFor="wateringFrequencyDays">Water Every (days) *</label>
            <input
              type="number"
              id="wateringFrequencyDays"
              name="wateringFrequencyDays"
              value={formData.wateringFrequencyDays}
              onChange={handleChange}
              min="1"
              max="30"
              required
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="fertilizingFrequencyDays">Fertilize Every (days)</label>
            <input
              type="number"
              id="fertilizingFrequencyDays"
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
            <label htmlFor="sunlight">Sunlight Requirements</label>
            <select
              id="sunlight"
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
            <label htmlFor="location">Location</label>
            <input
              type="text"
              id="location"
              name="location"
              value={formData.location}
              onChange={handleChange}
              placeholder="e.g., Living room window"
            />
          </div>
        </div>

        <div className="form-group">
          <label htmlFor="notes">Care Notes</label>
          <textarea
            id="notes"
            name="notes"
            value={formData.notes}
            onChange={handleChange}
            placeholder="Any special care instructions or notes..."
            rows="3"
          />
        </div>

        <button 
          type="submit" 
          className="submit-button"
          disabled={isLoading}
        >
          {isLoading ? "Adding Plant..." : "Add Plant"}
        </button>
      </form>
    </div>
  );
};

export default PlantForm;
