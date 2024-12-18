<template>
  <div class="update-reservation-form">
    <h1>Edit Reservation</h1>
    <div class="reservation-image-container">
      <img src="@/assets/Reservation-1.png" alt="Reservation" class="reservation-image" />
    </div>
    <div v-if="reservation">
      <form @submit.prevent="updateReservation">
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
          <input
            type="datetime-local"
            id="startTime"
            v-model="reservation.reservationStart"
            required
          />
        </div>

        <div class="form-group">
          <label for="endTime">Reservation End Time:</label>
          <input
            type="datetime-local"
            id="endTime"
            v-model="reservation.reservationEnd"
            required
          />
        </div>

        <div class="form-group">
          <label for="numberOfGuests">Number of Guests:</label>
          <input
            type="number"
            id="numberOfGuests"
            v-model="reservation.numberOfGuests"
            min="1"
            required
          />
        </div>

        <div class="form-group">
          <label for="specialRequests">Special Requests (Optional):</label>
          <textarea
            id="specialRequests"
            v-model="reservation.specialRequests"
            rows="3"
          ></textarea>
        </div>
        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>

        <button type="submit" class="submit-button" :disabled="loading">Update Reservation</button>
      </form>
      <div class="button-container">
        <button @click="goBack" class="back-button">Back to Reservation Details</button>
      </div>
    </div>
    <p v-else>Loading reservation details...</p>
  </div>
</template>


<script>
import axios from "axios";

export default {
  name: "ReservationEdit",
  props: ["tableId", "reservationId"],
  data() {
    return {
      reservation: null,
      errorMessage: "",
      loading: false, 
    };
  },
  methods: {
    async fetchReservationDetails() {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(
          `https://restauranttablereservationsystem.azurewebsites.net/api/tables/${this.tableId}/reservations/${this.reservationId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        this.reservation = response.data;
      } catch (error) {
        console.error("Failed to fetch reservation details:", error);
        alert("Error loading reservation details.");
      }
    },
       async updateReservation() {
      this.loading = true;
      this.errorMessage = "";

      try {
        const token = localStorage.getItem("token");
        await axios.put(
          `https://restauranttablereservationsystem.azurewebsites.net/api/tables/${this.tableId}/reservations/${this.reservationId}`,
          this.reservation,
          {
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
          }
        );
        alert("Reservation updated successfully!");
        this.$router.push({
          name: "ReservationDetails",
          params: { tableId: this.tableId, reservationId: this.reservationId },
        });
      } catch (error) {
        if (error.response && error.response.status === 422) {
          this.errorMessage = error.response.data;
        } else {
          this.errorMessage = "Error updating reservation. Please try again.";
        }
        console.error("Failed to update reservation:", error);
      } finally {
        this.loading = false;
      }
    },
    goBack() {
      this.$router.push({
        name: "ReservationDetails",
        params: { tableId: this.tableId, reservationId: this.reservationId },
      });
    },
  },
  created() {
    this.fetchReservationDetails();
  },
};
</script>

<style>
.update-reservation-form {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  font-family: "Arial", sans-serif;
}

.update-reservation-form h1 {
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
