<template>
  <nav>
    <div class="nav-desktop-menu">
      <router-link to="/">Home</router-link>
      <router-link to="/restaurants">Restaurants</router-link>
    </div>
    <div class="nav-right-menu">
    <template v-if="!authState.isLoggedIn">
        <router-link to="/register">Register</router-link>
        <router-link to="/login">Login</router-link>
      </template>
      <template v-else>
        <span class="logged-in-user">Logged in as : {{ authState.username }}</span>
        <router-link to="/profile" class="profile-link">Profile</router-link>
        <button @click="logout" class="logout-button">Logout</button>
      </template>
    </div>
    <div class="nav-mobile-menu">
      <button @click="toggleMenu">☰</button>
      <div v-if="showMenu">
        <router-link to="/">Home</router-link>
        <router-link to="/restaurants">Restaurants</router-link>
        <router-link v-if="!authState.isLoggedIn" to="/register">Register</router-link>
        <router-link v-if="!authState.isLoggedIn" to="/login">Login</router-link>
        <div v-else>
          <span class="logged-in-user">Logged in as : {{ authState.username }}</span>
          <router-link to="/profile" class="profile-link">Profile</router-link>
          <button @click="logout" class="logout-button">Logout</button>
        </div>
      </div>
    </div>
  </nav>
</template>

<script>
import { authState } from '../state/authState';
import axios from "axios";

export default {
  name: "AppNavbar",
  data() {
    return {
      showMenu: false,
      isLoggedIn: false, 
      username: "", 
    };
  },
  setup() {
    return { authState };
  },
  methods: {
    toggleMenu() {
      this.showMenu = !this.showMenu;
    },
   async logout() {
  const refreshToken = localStorage.getItem("refreshToken");
  const token = localStorage.getItem("token"); 

  if (refreshToken && token) {
    try {
      const response = await axios.post(
        "https://restauranttablereservationsystem.azurewebsites.net/api/users/logout",
        refreshToken,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`, 
          },
        }
      );
      console.log("Logout successful. Backend Response:", response.data);
    } catch (error) {
      if (error.response) {
        console.error(
          "Logout API error:",
          error.response.status,
          error.response.data
        );
      } else if (error.request) {
        console.error("No response received:", error.request);
      } else {
        console.error("Axios request error:", error.message);
      }
    }
  } else {
    console.error("No refresh token or access token found in local storage.");
  }

      localStorage.removeItem("token");
      localStorage.removeItem("refreshToken");
      localStorage.removeItem("tokenExpiration");
      localStorage.removeItem("username");
      this.isLoggedIn = false;
      this.username = ""; 
     this.$router.push("/").then(() => {
    this.$router.go(0);
  });
    },
  },
  created() {
    const token = localStorage.getItem("token");
    const storedUsername = localStorage.getItem("username");
    if (token && storedUsername) {
      this.isLoggedIn = true;
      this.username = storedUsername;
    }
  },
};
</script>

<style>
nav {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 15px;
  margin-left: 10px;
  margin-right: 10px;
  margin-bottom: 5px;
  justify-content: space-between;
}
.nav-desktop-menu {
  display: flex;
  gap: 15px;
}
.nav-desktop-menu a {
  color: white;
  text-decoration: none;
  font-family: 'Roboto';
  font-size: 20px;
}
.nav-desktop-menu a:hover {
   color:black;
}
.nav-right-menu {
  display: flex;
  gap: 15px;
  margin-left: auto; 
}
.nav-right-menu a {
  color: white;
  text-decoration: none;
  font-family: 'Roboto';
  font-size: 20px;
}

.nav-right-menu a:hover {
  color: black;
}
.logout-button {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 8px 15px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.logout-button:hover {
  background-color: #d32f2f;
}
.logged-in-user {
   color: white;
  text-decoration: none;
  font-family: 'Roboto';
  font-size: 20px;
  margin-top : 5px;
}
.nav-mobile-menu {
  display: none;
}
.nav-mobile-menu a {
  display: flex;
  color: white;
  text-decoration: none;
}
.nav-mobile-menu div {
  display: flex;
  flex-direction: column; 
  gap: 10px;
}
.nav-mobile-menu a:hover {
   color:black;
}
@media (max-width: 768px) {
  .nav-desktop-menu,
  .nav-right-menu {
    display: none;
  }
  .nav-mobile-menu {
    display: block;
  }
}
.profile-link {
  color: white;
  text-decoration: none;
  font-family: 'Roboto';
  font-size: 20px;
  margin-right: 15px; 
  margin-top : 5px;
}

.profile-link:hover {
  color: black;
}
</style>
