<template>
  <div>
    <h1>Restaurants</h1>
    <div v-for="restaurant in restaurants" :key="restaurant.restaurantId" class="restaurant-card">
      <h2>{{ restaurant.name }}</h2>
      <p>{{restaurant.location}}</p>
      <button @click="fetchDetails(restaurant.restaurantId)">View Details</button>
      <template v-if="isLoggedIn && isAdmin">
        <button @click="editRestaurant(restaurant.restaurantId)" class="editt-button">Edit Restaurant</button>
        <button @click="deleteRestaurant(restaurant.restaurantId)" class="deletee-button">Delete Restaurant</button>
      </template>
    </div>
    <div v-if="isLoggedIn && isAdmin" class="add-restaurant-container">
      <button @click="addRestaurant" class="add-restaurant-button">Add Restaurant</button>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: "AppRestaurants",
  data() {
    return {
      restaurants: [],   
      isLoggedIn: !!localStorage.getItem("token"), 
      isAdmin: localStorage.getItem("username") === "admin",    
    };
  },
  methods: {
    fetchDetails(restaurantId) {
      this.$router.push({ name: 'RestaurantDetails', params: { restaurantId } }); 
    },
    addRestaurant() {
      this.$router.push({ name: 'AddRestaurant' });
    },
    editRestaurant(restaurantId) {
      this.$router.push({ name: "EditRestaurant", params: { restaurantId } });
    },
    async deleteRestaurant(restaurantId) {
      const confirmDelete = confirm("Are you sure you want to delete this restaurant?");
      if (confirmDelete) {
        try {
          const token = localStorage.getItem("token");
          await axios.delete(`https://restauranttablereservationsystem.azurewebsites.net/api/restaurants/${restaurantId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
          alert("Restaurant deleted successfully!");
          this.restaurants = this.restaurants.filter((r) => r.restaurantId !== restaurantId); // Update UI
        } catch (error) {
          console.error("Error deleting restaurant:", error);
          alert("Failed to delete the restaurant. Please try again.");
        }
      }
    },
  },
  created() {
    axios.get('https://restauranttablereservationsystem.azurewebsites.net/api/restaurants')
      .then(response => {
        this.restaurants = response.data;
      })
      .catch(error => console.error(error));
  },
};
</script>

<style>
.restaurant-card {
  border: 1px solid #ddd; 
  padding: 20px; 
  margin-bottom: 20px; 
  border-radius: 10px; 
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
  transition: transform 0.2s ease, box-shadow 0.2s ease; 
  background-color: #ffffff; 
}

.restaurant-card h2 {
  font-family: 'Roboto', sans-serif; 
  color: #3f51b5; 
  font-size: 1.5rem;
  margin-bottom: 10px;
}

.restaurant-card button {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 10px 15px;
  border-radius: 5px;
  cursor: pointer;
  margin-right: 10px;
  transition: background-color 0.2s ease;
}

.restaurant-card button:hover {
  background-color: #388e3c;
}

.restaurant-card:hover {
  transform: translateY(-5px); 
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2); 
}

.restaurant-card p {
  color: #666; 
  font-family: 'Roboto', sans-serif; 
  font-size: 1rem;
  line-height: 1.5; 
}
.add-restaurant-container {
  text-align: center;
  margin-top: 20px;
}

.add-restaurant-button {
  background-color: #2196f3;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.add-restaurant-button:hover {
  background-color: #1976d2;
}
.restaurant-card .editt-button {
  background-color: #ff9800; 
}

.restaurant-card .editt-button:hover {
  background-color: #e68900; 
}

.restaurant-card .deletee-button {
  background-color: #f44336;
}

.restaurant-card .deletee-button:hover {
  background-color: #d32f2f;
}
</style>
