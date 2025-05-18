import React, { useEffect, useState } from "react";
import { getPlants } from "../api";

const PlantList = ({ refresh }) => {
  const [plants, setPlants] = useState([]);

  useEffect(() => {
    async function fetchData() {
      const data = await getPlants();
      setPlants(data);
    }
    fetchData();
  }, [refresh]);

  return (
    <div>
      <h2>My Plants</h2>
      <ul>
        {plants.map((plant) => (
          <li key={plant._id}>
            🌿 <strong>{plant.name}</strong> — Last Watered: {plant.lastWatered}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default PlantList;
