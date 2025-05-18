const API_BASE_URL = "http://localhost:5177/api";

export async function getPlants() {
  try {
    const response = await fetch(`${API_BASE_URL}/plant`);
    if (!response.ok) throw new Error("Failed to fetch plants");
    return await response.json();
  } catch (error) {
    console.error("API error:", error);
    return [];
  }
}

export async function addPlant(plant) {
  try {
    const response = await fetch(`${API_BASE_URL}/plant`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(plant),
    });

    if (!response.ok) throw new Error("Failed to add plant");
    return await response.json();
  } catch (error) {
    console.error("API error:", error);
  }
}

