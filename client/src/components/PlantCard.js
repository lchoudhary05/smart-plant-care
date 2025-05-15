export default function PlantCard({ plant }) {
    return (
      <div style={{ border: "1px solid #2f855a", borderRadius: 8, padding: 16, margin: 8 }}>
        <h2 style={{ color: "#276749" }}>{plant.name}</h2>
        <p>Last watered: {plant.lastWatered}</p>
        <p>Sunlight: {plant.sunlight}</p>
      </div>
    );
  }
  