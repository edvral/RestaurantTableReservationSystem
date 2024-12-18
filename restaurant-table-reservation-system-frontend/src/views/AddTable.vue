<template>
  <div class="add-table-form">
    <h1>Add Table for {{ restaurantName }}</h1>
     <div class="table-image-container">
      <img src="@/assets/table.jpg" alt="Table" class="table-image" />
    </div>
    <form @submit.prevent="submitTable">
      <div class="form-group">
        <label for="tableNumber">Table Number:</label>
        <input
          type="number"
          id="tableNumber"
          v-model="table.tableNumber"
          min="1"
          required
        />
      </div>
      <div class="form-group">
        <label for="capacity">Capacity:</label>
        <input
          type="number"
          id="capacity"
          v-model="table.capacity"
          min="1"
          required
        />
      </div>
      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
      <div class="button-container">
        <button type="submit" class="submit-button">Add Table</button>
        <button @click="goBack" type="button" class="back-button">Back to Tables</button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "AddTable",
  props: ["restaurantId"],
  data() {
    return {
      restaurantName: "",
      table: {
        tableNumber: "",
        capacity: "",
      },
       errorMessage: "", 
    };
  },
  methods: {
    async fetchRestaurantDetails() {
      try {
        const response = await axios.get(
          `https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.restaurantId}`
        );
        this.restaurantName = response.data.name;
      } catch (error) {
        console.error("Error fetching restaurant details:", error);
        alert("Failed to fetch restaurant details.");
      }
    },
    async submitTable() {
      try {
        const token = localStorage.getItem("token");
        await axios.post(
          `https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.restaurantId}/tables`,
          this.table,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        alert("Table added successfully!");
        this.$router.push({
          name: "RestaurantTables",
          params: { restaurantId: this.restaurantId },
        });
      }catch (error) {
        if (error.response && error.response.status === 422) {
          // Show validation error to the user
          this.errorMessage = error.response.data;
        } else {
          console.error("Error adding table:", error);
          alert("Failed to add table. Please try again.");
        }
      }
    },
    goBack() {
      this.$router.push({
        name: "RestaurantTables",
        params: { restaurantId: this.restaurantId },
      });
    },
  },
  created() {
    this.fetchRestaurantDetails();
  },
};
</script>

<style>
.add-table-form {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  font-family: "Arial", sans-serif;
}

.add-table-form h1 {
  text-align: center;
  font-size: 1.8rem;
  color: #333;
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1rem;
}

.submit-button {
  display: inline-block;
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  font-size: 1rem;
  cursor: pointer;
  margin-right: 10px;
  transition: background-color 0.3s ease;
}

.submit-button:hover {
  background-color: #388e3c;
}

.back-button {
  display: inline-block;
  background-color: #3f51b5;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.back-button:hover {
  background-color: #2c387e;
}
.table-image-container {
  text-align: center;
  margin-bottom: 20px;
}

.table-image {
  max-width: 100%;
  height: auto;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
