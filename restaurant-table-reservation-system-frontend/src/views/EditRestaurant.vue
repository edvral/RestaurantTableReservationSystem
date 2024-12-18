<template>
  <div class="edit-restaurant-form">
    <h1>Edit Restaurant {{restaurant.name}}</h1>
    <div class="restaurantt-image-container">
      <img src="@/assets/resto.jpg" alt="Restaurant" class="restaurantt-image" />
    </div>
    <form @submit.prevent="updateRestaurant">
      <div class="form-group">
        <label for="name">Name:</label>
        <input
          type="text"
          id="name"
          v-model="restaurant.name"
          required
        />
      </div>
      <div class="form-group">
        <label for="location">Location:</label>
        <input
          type="text"
          id="location"
          v-model="restaurant.location"
          required
        />
      </div>
      <div class="form-group">
        <label for="phoneNumber">Phone Number:</label>
        <input
          type="text"
          id="phoneNumber"
          v-model="restaurant.phoneNumber"
          required
        />
      </div>
      <div class="form-group">
        <label for="email">Email:</label>
        <input
          type="email"
          id="email"
          v-model="restaurant.email"
          required
        />
      </div>
      <div class="form-group">
        <label for="description">Description:</label>
        <textarea
          id="description"
          v-model="restaurant.description"
          required
        ></textarea>
      </div>
      <div class="form-group">
        <label for="openingHours">Opening Hours:</label>
        <input
          type="text"
          id="openingHours"
          v-model="restaurant.openingHours"
          required
        />
      </div>
      <div class="button-container">
        <button type="submit" class="submit-button">Update Restaurant</button>
        <button @click="goBack" type="button" class="back-button">Back to Restaurants</button>
      </div>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "EditRestaurant",
  data() {
    return {
      restaurant: {
        name: "",
        location: "",
        phoneNumber: "",
        email: "",
        description: "",
        openingHours: "",
      },
    };
  },
  methods: {
    goBack() {
      this.$router.push("/restaurants");
    },
    async fetchRestaurantDetails() {
      try {
        const response = await axios.get(
          `https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.$route.params.restaurantId}`
        );
        this.restaurant = response.data;
      } catch (error) {
        console.error("Error fetching restaurant details:", error);
        alert("Failed to fetch restaurant details.");
      }
    },
    async updateRestaurant() {
      try {
        const token = localStorage.getItem("token");
        await axios.put(
          `https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.$route.params.restaurantId}`,
          this.restaurant,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        alert("Restaurant updated successfully!");
        this.$router.push({ name: "RestaurantDetails", params: { restaurantId: this.$route.params.restaurantId } });
      } catch (error) {
        console.error("Error updating restaurant:", error);
        alert("Failed to update restaurant. Please try again.");
      }
    },
  },
  created() {
    this.fetchRestaurantDetails();
  },
};
</script>

<style>
.edit-restaurant-form {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  font-family: "Arial", sans-serif;
}

.edit-restaurant-form h1 {
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

.form-group input,
.form-group textarea {
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

.back-button {
  margin-top: 10px;
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
