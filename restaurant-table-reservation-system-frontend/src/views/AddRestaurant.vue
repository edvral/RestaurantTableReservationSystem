<template>
  <div class="add-restaurant">
    <h1>Add a New Restaurant</h1>
    <div class="restaurantt-image-container">
      <img src="@/assets/resto.jpg" alt="Restaurant" class="restaurantt-image" />
    </div>
    <form @submit.prevent="submitRestaurant">
      <div class="form-group">
        <label for="name">Restaurant Name:</label>
        <input type="text" id="name" v-model="restaurant.name" required />
      </div>

      <div class="form-group">
        <label for="location">Location:</label>
        <input type="text" id="location" v-model="restaurant.location" required />
      </div>

      <div class="form-group">
        <label for="phoneNumber">Phone Number:</label>
        <input type="tel" id="phoneNumber" v-model="restaurant.phoneNumber" required />
      </div>

      <div class="form-group">
        <label for="email">Email:</label>
        <input type="email" id="email" v-model="restaurant.email" required />
      </div>

      <div class="form-group">
        <label for="openingHours">Opening Hours:</label>
        <input type="text" id="openingHours" v-model="restaurant.openingHours" required />
      </div>

      <div class="form-group">
        <label for="description">Description:</label>
        <textarea id="description" v-model="restaurant.description" rows="4" required></textarea>
      </div>
      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>

      <button type="submit" class="submit-button">Add Restaurant</button>
    </form>
    <div class="button-container">
      <button @click="goBack" class="back-button">Back to Restaurants</button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "AddRestaurant",
  data() {
    return {
      restaurant: {
        name: "",
        location: "",
      },
      errorMessage: "",
    };
  },
  methods: {
    async submitRestaurant() {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.post(
          "https://restauranttablereservationsystem.azurewebsites.net/api/restaurants",
          this.restaurant,
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (response.status === 201) {
          alert("Restaurant added successfully!");
          this.$router.push("/restaurants"); 
        }
      } catch (error) {
        if (error.response && error.response.status === 422) {
          this.errorMessage = error.response.data; 
        } else {
          console.error("Error adding restaurant:", error);
          alert("Failed to add restaurant. Please try again.");
        }
      }
    },
    goBack() {
      this.$router.push("/restaurants"); 
    },
  },
};
</script>

<style>
.add-restaurant {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.add-restaurant h1 {
  text-align: center;
  font-size: 1.8rem;
  color: #333;
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
  color: #555;
}

.form-group input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1rem;
}

.submit-button {
  display: block;
  width: 100%;
  padding: 12px;
  background-color: #4caf50;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.submit-button:hover {
  background-color: #388e3c;
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
.restaurantt-image-container {
  text-align: center;
  margin-bottom: 20px;
}

.restaurantt-image {
  max-width: 100%;
  height: auto;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
