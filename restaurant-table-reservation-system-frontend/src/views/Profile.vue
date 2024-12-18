<template>
  <div class="profile-container">
    <h1>Profile</h1>
    <div v-if="user">
      <p><strong>Username:</strong> {{ user.username }}</p>
      <p><strong>Email:</strong> {{ user.email }}</p>
      <p><strong>Role:</strong> {{ user.role }}</p>
    </div>
    <p v-else>Loading user information...</p>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "UserProfile",
  data() {
    return {
      user: null,
    };
  },
  async created() {
    try {
      const userId = localStorage.getItem("userId");
      const token = localStorage.getItem("token");

      if (!userId || !token) {
        throw new Error("User not logged in or token missing.");
      }

      const response = await axios.get(`https://restauranttablereservationsystem.azurewebsites.net/api/users/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      this.user = response.data;
    } catch (error) {
      console.error("Error fetching user data:", error);
      alert("Failed to load profile information.");
    }
  },
};
</script>

<style>
.profile-container {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  font-family: "Arial", sans-serif;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.profile-container h1 {
  text-align: center;
  font-size: 1.8rem;
  color: black;
  margin-bottom: 20px;
}

.profile-container p {
  font-size: 1rem;
  color: #555;
  margin: 10px 0;
}

.profile-container strong {
  color: black;
}
</style>
