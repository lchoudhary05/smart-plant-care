import { useState } from "react";

export default function AddPlantForm({ onAdd }) {
  const [name, setName] = useState("");
  const [lastWatered, setLastWatered] = useState("");
  const [sunlight, setSunlight] = useState("");

  function handleSubmit(e) {
    e.preventDefault();
    if (!name) return alert("Please enter a plant name");

    onAdd({ name, lastWatered, sunlight });

    // Reset form
    setName("");
    setLastWatered("");
    setSunlight("");
  }

  return (
    <form onSubmit={handleSubmit} style={{ margin: "20px", padding: "10px", border: "1px solid #276749", borderRadius: 8 }}>
      <h3>Add a New Plant</h3>
      <input
        type="text"
        placeholder="Plant Name"
        value={name}
        onChange={e => setName(e.target.value)}
        required
        style={{ marginBottom: 8, padding: 4, width: "100%" }}
      />
      <input
        type="date"
        placeholder="Last Watered"
        value={lastWatered}
        onChange={e => setLastWatered(e.target.value)}
        style={{ marginBottom: 8, padding: 4, width: "100%" }}
      />
      <input
        type="text"
        placeholder="Sunlight (e.g. indirect bright light)"
        value={sunlight}
        onChange={e => setSunlight(e.target.value)}
        style={{ marginBottom: 8, padding: 4, width: "100%" }}
      />
      <button type="submit" style={{ backgroundColor: "#276749", color: "white", padding: "8px 12px", border: "none", borderRadius: 4 }}>
        Add Plant
      </button>
    </form>
  );
}
