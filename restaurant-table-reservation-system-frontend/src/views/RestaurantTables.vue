<template>
  <div class="restaurant-tables">
    <h1>Tables for {{ restaurantName }}</h1>
    <div v-if="tables.length" class="table-list">
      <div v-for="table in tables" :key="table.tableId" class="table-card">
        <p><strong>Table Number:</strong> {{ table.tableNumber }}</p>
        <p><strong>Capacity:</strong> {{ table.capacity }}</p>
        <button @click="goToReservations(table.tableId)" class="view-reservations-button">View Reservations</button>
        <template v-if="isLoggedIn && isAdmin">
          <button @click="editTable(table.tableId)" class="edit-table-button">Edit Table</button>
          <button @click="deleteTable(table.tableId)" class="delete-table-button">Delete Table</button>
        </template>
      </div>
    </div>
    <p v-else>No tables available for this restaurant.</p>
    <div v-if="isLoggedIn && isAdmin" class="add-table-container">
      <button @click="addTable" class="add-table-button">Add Table</button>
    </div>
    <div class="button-container">
      <button @click="goBack" class="back-button">Back to Restaurant</button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "RestaurantTables",
  props: ["restaurantId"],
  data() {
    return {
      restaurantName: "", 
      tables: [],
      isLoggedIn: !!localStorage.getItem("token"),
      isAdmin: localStorage.getItem("username") === "admin",
    };
  },
  methods: {
    goBack() {
      this.$router.push(`/restaurants/${this.restaurantId}`);
    },
    addTable() {
      this.$router.push({
        name: "AddTable",
        params: { restaurantId: this.restaurantId },
      });
    },
    goToReservations(tableId) {
    localStorage.setItem("currentRestaurantId", this.restaurantId);
    this.$router.push({
    name: "TableReservations",
    params: { tableId },
  });
},
 editTable(tableId) {
  localStorage.setItem("currentRestaurantName", this.restaurantName);
      this.$router.push({
        name: "EditTable",
        params: { tableId },
      });
    },
    async deleteTable(tableId) {
      const confirmDelete = confirm("Are you sure you want to delete this table?");
      if (confirmDelete) {
        try {
          const token = localStorage.getItem("token");
          await axios.delete(
            `https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.restaurantId}/tables/${tableId}`,
            {
              headers: {
                Authorization: `Bearer ${token}`,
              },
            }
          );
          alert("Table deleted successfully!");
          this.tables = this.tables.filter((t) => t.tableId !== tableId);
        } catch (error) {
          console.error("Error deleting table:", error);
          alert("Failed to delete the table. Please try again.");
        }
      }
    },
  },
  created() {
    axios
      .get(`https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.restaurantId}`)
      .then((response) => {
        this.restaurantName = response.data.name;
      })
      .catch((error) =>
        console.error(`Error fetching restaurant details: ${error}`)
      );

    axios
      .get(`https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.restaurantId}/tables`)
      .then((response) => {
        this.tables = response.data;
      })
      .catch((error) =>
        console.error(`Error fetching tables: ${error}`)
      );
  },
};
</script>

<style>
.restaurant-tables {
  max-width: 800px; 
  margin: 20px auto; 
  padding: 20px;
  font-family: 'Arial', sans-serif;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.restaurant-tables h1 {
  font-size: 1.8rem;
  color: black;
  text-align: center;
  margin-bottom: 20px;
}

.table-list {
  margin-top: 20px;
}

.table-card {
  position: relative;
  border: 1px solid #ddd;
  padding: 10px;
  margin-bottom: 10px;
  border-radius: 5px;
  background-color: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.3s ease, transform 0.3s ease; 
}

.table-card:hover {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); 
  transform: translateY(-5px);
}

.table-card p {
  font-size: 1rem;
  color: #555;
  margin: 5px 0;
}

.button-container {
  text-align: center;
  margin-top: 20px;
}

.back-button {
  background-color: #3f51b5;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.back-button:hover {
  background-color: #2c387e;
}

.view-reservations-button {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
  position: absolute; 
  right: 10px; 
  top: 50%; 
  transform: translateY(-50%); 
}

.view-reservations-button:hover {
  background-color: #388e3c; 
}
.edit-table-button {
  background-color: #ff9800; /* Orange */
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
  position: absolute;
  right: 140px; /* Adjust position to avoid overlap */
  top: 50%;
  transform: translateY(-50%);
  margin-right: 187px;
}

.edit-table-button:hover {
  background-color: #e68900;
}

.delete-table-button {
  background-color: #f44336; /* Red */
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
  position: absolute;
  right: 10px; /* Adjust position to avoid overlap */
  top: 50%;
  transform: translateY(-50%);
  margin-right: 180px;
}

.delete-table-button:hover {
  background-color: #d32f2f;
}
.add-table-container {
  text-align: center;
  margin-top: 20px;
}

.add-table-button {
  background-color: #2196f3;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.add-table-button:hover {
  background-color: #1976d2;
}
</style>
