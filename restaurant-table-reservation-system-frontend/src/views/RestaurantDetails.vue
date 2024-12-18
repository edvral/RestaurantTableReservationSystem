<template>
  <div class="restaurant-details">
    <h1 class="restaurant-name">{{ restaurant.name }}</h1>
    <div class="details-card">
      <p><strong>Location:</strong> {{ restaurant.location }}</p>
      <p><strong>Phone:</strong> {{ restaurant.phoneNumber }}</p>
      <p><strong>Email:</strong> {{ restaurant.email }}</p>
      <p><strong>Opening Hours:</strong> {{ restaurant.openingHours }}</p>
      <p><strong>Description:</strong> {{ restaurant.description }}</p>
    </div>
    <div class="button-container">
      <button @click="goToTables" class="view-tables-button">View Tables</button>
      <button @click="goBack" class="back-button">Back to List</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'RestaurantDetails',
  props: ['restaurantId'],
  data() {
    return {
      restaurant: {},
    };
  },
  methods: {
    goBack() {
      this.$router.push("/restaurants"); 
    },
    goToTables() {
      this.$router.push(`/restaurants/${this.restaurantId}/tables`);
    },
  },
  created() {
    axios
      .get(`https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${this.restaurantId}`)
      .then((response) => {
        this.restaurant = response.data;
      })
      .catch((error) => console.error(`Error fetching restaurant details: ${error}`));
  },
};
</script>

<style>
.restaurant-details {
  max-width: 700px; 
  margin: 20px auto; 
  padding: 20px;
  font-family: 'Arial', sans-serif; 
  border: 1px solid #ddd; 
  border-radius: 10px;
  background-color: #f9f9f9; 
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
}

.restaurant-name {
  font-size: 2rem; 
  color: #333; 
  margin-bottom: 15px;
  text-align: center; 
}

.details-card p {
  font-size: 1rem; 
  color: black; 
  margin: 10px 0; 
}

.details-card p strong {
  font-weight: bold; 
}

.button-container {
  text-align: center;
  margin-top: 20px;
  display: flex;
  flex-direction: column; 
  align-items: center; 
  gap: 10px; 
}

.view-tables-button {
  background-color: #4caf50; 
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.view-tables-button:hover {
  background-color: #388e3c; 
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
</style>