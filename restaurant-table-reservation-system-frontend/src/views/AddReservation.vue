<template>
  <div class="add-reservation-form">
    <h1>Add Reservation for Table {{ tableId }}</h1>
    <div class="reservation-image-container">
      <img src="@/assets/Reservation-1.png" alt="Reservation" class="reservation-image" />
    </div>
    <form @submit.prevent="submitReservation">
      <!-- Guest Name -->
      <div class="form-group">
        <label for="guestName">Guest Name:</label>
        <input type="text" id="guestName" v-model="reservation.guestName" required />
      </div>

      <div class="form-group">
        <label for="guestPhone">Guest Phone Number:</label>
        <input type="tel" id="guestPhone" v-model="reservation.guestPhoneNumber" required />
      </div>

      <div class="form-group">
        <label for="startTime">Reservation Start Time:</label>
        <input type="datetime-local" id="startTime" v-model="reservation.reservationStart" required />
      </div>

      <div class="form-group">
        <label for="endTime">Reservation End Time:</label>
        <input type="datetime-local" id="endTime" v-model="reservation.reservationEnd" required />
      </div>

      <div class="form-group">
        <label for="numberOfGuests">Number of Guests:</label>
        <input type="number" id="numberOfGuests" v-model="reservation.numberOfGuests" min="1" required />
      </div>

      <div class="form-group">
        <label for="specialRequests">Special Requests (Optional):</label>
        <textarea id="specialRequests" v-model="reservation.specialRequests" rows="3"></textarea>
      </div>

      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>

      <button type="submit" class="submit-button" :disabled="loading">Add Reservation</button>
    </form>
    <div class="button-container">
      <button @click="goBackToReservations" class="back-to-reservations-button">
        Back to Reservations
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "AddReservation",
  props: ["tableId"],
  data() {
    return {
      reservation: {
        guestName: "", 
        guestPhoneNumber: "", 
        reservationStart: "", 
        reservationEnd: "", 
        numberOfGuests: 1, 
        specialRequests: "", 
      },
      errorMessage: "",
      loading: false,
    };
  },
 methods: {
    goBackToReservations() {
    this.$router.back();
  },
    async submitReservation() {
      this.loading = true;
      this.errorMessage = "";
      try {
        const token = localStorage.getItem("token");
        const response = await axios.post(
          `https://restauranttablereservationsystem.azurewebsites.net/api/tables/${this.tableId}/reservations`,
           this.reservation,
    {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
        },
    }
        );

        if (response.status === 201 || response.status === 200) {
          alert("Reservation added successfully!");
          this.$router.push({ name: "TableReservations", params: { tableId: this.tableId } });
        }
      } catch (error) {
        if (error.response && error.response.status === 422) {
          this.errorMessage = error.response.data; // Show backend error
        } else {
          this.errorMessage = "An unexpected error occurred. Please try again later.";
        }
        console.error("Failed to add reservation:", error);
      } finally {
        this.loading = false; // Re-enable the submit button
      }
    },
  },
};
</script>

<style>
.add-reservation-form {
  max-width: 600px; 
  margin: 20px auto; 
  padding: 20px; 
  border: 1px solid #ddd; 
  border-radius: 10px; 
  background-color: #f9f9f9; 
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
}

.add-reservation-form h1 {
  text-align: center; 
  font-size: 1.8rem; 
  color: #black; 
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
  text-align: center; 
  transition: background-color 0.3s ease, transform 0.2s ease; 
}

.submit-button:hover {
  background-color: #388e3c;
  transform: translateY(-2px); 
}

.submit-button:active {
  background-color: #2e7d32; 
  transform: translateY(0); 
}
.back-to-reservations-button {
  background-color: #3f51b5;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.back-to-reservations-button:hover {
   background-color: #2c387e;
}
.reservation-image-container {
  text-align: center;
  margin-bottom: 20px;
}

.reservation-image {
  max-width: 100%;
  height: auto;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
</style>
