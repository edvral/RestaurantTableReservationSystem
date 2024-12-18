<template>
  <div class="login-form">
    <h1>Login</h1>
    <form @submit.prevent="login">
      <div class="form-group">
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="form.username" required />
      </div>
      <div class="form-group">
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="form.password" required />
      </div>
      <button type="submit" class="login-button">Login</button>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    </form>
  </div>
</template>

<script>
import axios from "axios";
import { authState } from "../state/authState";
import { jwtDecode } from "jwt-decode";

export default {
  name: "AppLogin",
  data() {
    return {
      form: {
        username: "",
        password: "",
      },
      errorMessage: "",
    };
  },
  methods: {
    async login() {
      try {
        const response = await axios.post(
          "https://restauranttablereservationsystem.azurewebsites.net/api/users/login",
          this.form
        );

        if (response.status === 200) {
          const { token, expiration, refreshToken } = response.data;

           const decodedToken = jwtDecode(token);
           const userId = decodedToken.userId;

          const expirationTimestamp = Math.floor(new Date(expiration).getTime() / 1000);

          localStorage.setItem("token", token);
          localStorage.setItem("refreshToken", refreshToken);
          localStorage.setItem("tokenExpiration", expirationTimestamp); 
          localStorage.setItem("username", this.form.username);
          localStorage.setItem("userId", userId); 

          authState.isLoggedIn = true;
          authState.username = this.form.username;

          this.startRefreshTimer();
          this.$router.push("/");
        }
      } catch (error) {
        if (error.response && error.response.status === 401) {
          this.errorMessage = "Invalid username or password.";
        } else {
          this.errorMessage = "An error occurred during login.";
        }
      }
    },

    async refreshToken() {
  try {
    const refreshToken = localStorage.getItem("refreshToken");

    if (!refreshToken) {
      throw new Error("No refresh token available.");
    }

    const response = await axios.post(
      "https://restauranttablereservationsystem.azurewebsites.net/api/users/refresh",
      refreshToken,
      {
        headers: { "Content-Type": "application/json" },
      }
    );

    if (response.status === 200) {
      const { token: newToken, refreshToken: newRefreshToken } = response.data;

      const newExpiration = Math.floor(Date.now() / 1000) + 3600; 

      localStorage.setItem("token", newToken);
      localStorage.setItem("refreshToken", newRefreshToken);
      localStorage.setItem("tokenExpiration", newExpiration);

      console.log("Token refreshed successfully.");
      return true; 
    }
  } catch (error) {
    console.error("Failed to refresh token:", error);
    alert("Your session has expired. Please log in again.");
    this.logout();
    return false; 
  }
},

  startRefreshTimer() {
  const expiration = localStorage.getItem("tokenExpiration");
  const now = Math.floor(Date.now() / 1000); 

  if (!expiration) {
    console.warn("No token expiration found. Logging out...");
    this.logout();
    return;
  }

  const expirationTimestamp = parseInt(expiration, 10);

  if (isNaN(expirationTimestamp)) {
    console.warn("Invalid expiration time. Logging out...");
    this.logout();
    return;
  }

  if (expirationTimestamp > now) {
    const timeout = (expirationTimestamp - now - 300) * 1000; 
    console.log(`Scheduling token refresh in ${timeout / 1000} seconds.`);
    setTimeout(async () => {
      const success = await this.refreshToken(); 
      if (success) {
        this.startRefreshTimer();
      }
    }, timeout);
  } else {
    console.warn("Token has already expired. Logging out...");
    this.logout();
  }
},

    logout() {
      localStorage.removeItem("token");
      localStorage.removeItem("refreshToken");
      localStorage.removeItem("tokenExpiration");
      localStorage.removeItem("username");
      authState.isLoggedIn = false;
      authState.username = "";
       this.$router.push("/").then(() => {
    this.$router.go(0);
  });
    },
  },
  created() {
  if (this.$route.name !== "Login") {
    const expiration = localStorage.getItem("tokenExpiration");
    const now = Math.floor(Date.now() / 1000);

    if (expiration && parseInt(expiration, 10) > now) {
      console.log("Starting refresh timer...");
      this.startRefreshTimer();
    } else {
      console.warn("Token has already expired or is invalid. Logging out...");
      this.logout();
    }
  }
},
};
</script>

<style>
.login-form {
  max-width: 400px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.login-form h1 {
  text-align: center;
  color: black;
}

.form-group {
  margin-bottom: 15px;
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
}

.login-button {
  width: 100%;
  padding: 10px;
  background-color: #3f51b5;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.login-button:hover {
  background-color: #2c387e;
}

.error-message {
  margin-top: 10px;
  color: red;
  font-size: 0.9rem;
  text-align: center;
}
</style>
