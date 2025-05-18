import React, { useState } from "react";
import { addPlant } from "../api";

const PlantForm = ({ onPlantAdded }) => {
  const [name, setName] = useState("");
  const [lastWatered, setLastWatered] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    const newPlant = { name, lastWatered };
    await addPlant(newPlant);
    setName("");
    setLastWatered("");
    if (onPlantAdded) onPlantAdded(); // refresh plant list
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Add New Plant</h3>
      <input
        type="text"
        placeholder="Plant Name"
        value={name}
        onChange={(e) => setName(e.target.value)}
        required
      />
      <input
        type="date"
        placeholder="Last Watered"
        value={lastWatered}
        onChange={(e) => setLastWatered(e.target.value)}
        required
      />
      <button type="submit">Add Plant</button>
    </form>
  );
};

export default PlantForm;
