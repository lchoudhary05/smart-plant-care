import React, { useState } from "react";
import PlantForm from "./components/PlantForm";
import PlantList from "./components/PlantList";

function App() {
  const [refresh, setRefresh] = useState(false);

  return (
    <div className="App">
      <h1>Smart Plant Care</h1>
      <PlantForm onPlantAdded={() => setRefresh(!refresh)} />
      <PlantList refresh={refresh} />
    </div>
  );
}

export default App;
