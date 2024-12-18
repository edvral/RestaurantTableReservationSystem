<template>
  <div class="register-form">
    <h1>Register</h1>
    <form @submit.prevent="register">
      <div class="form-group">
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="form.username" required />
      </div>
      <div class="form-group">
        <label for="email">Email:</label>
        <input type="email" id="email" v-model="form.email" required />
      </div>
      <div class="form-group">
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="form.password" required />
      </div>
      <button type="submit" class="register-button">Register</button>
      <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "AppRegister",
  data() {
    return {
      form: {
        username: "",
        email: "",
        password: "",
      },
      errorMessage: "",
    };
  },
  methods: {
    async register() {
      try {
        const response = await axios.post(
          "https://restauranttablereservationsystem.azurewebsites.net/api/users/register",
          this.form
        );
        if (response.status === 201) {          
          this.$router.push("/login");
        }
      } catch (error) {
        if (error.response && error.response.status === 409) {
          this.errorMessage = error.response.data;
        } else if (error.response && error.response.status === 422) {
          this.errorMessage = error.response.data;
        } else {
          this.errorMessage = "An error occurred during registration.";
        }
      }
    },
  },
};
</script>

<style>
.register-form {
  max-width: 400px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.register-form h1 {
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

.register-button {
  width: 100%;
  padding: 10px;
  background-color: #4caf50;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.register-button:hover {
  background-color: #388e3c;
}

.error-message {
  margin-top: 10px;
  color: red;
  font-size: 0.9rem;
  text-align: center;
}
</style>
