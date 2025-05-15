import { useState } from "react";
import PlantCard from "./components/PlantCard";
import AddPlantForm from "./components/AddPlantForm";

function App() {
  const [plants, setPlants] = useState([
    { name: "Fiddle Leaf Fig", lastWatered: "2025-05-10", sunlight: "Indirect bright light" },
  ]);

  function addPlant(newPlant) {
    setPlants(prev => [...prev, newPlant]);
  }

  return (
    <div>
      <h1>🌿 Smart Plant Care App</h1>
      <AddPlantForm onAdd={addPlant} />
      {plants.map((plant, index) => (
        <PlantCard key={index} plant={plant} />
      ))}
    </div>
  );
}

export default App;
