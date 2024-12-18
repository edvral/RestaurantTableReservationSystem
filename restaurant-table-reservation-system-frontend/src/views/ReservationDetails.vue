<template>
  <div class="reservation-details">
    <h1>Reservation Details</h1>
    <div v-if="reservation" class="details-card">
      <p><strong>Reservation Id:</strong> {{ reservation.reservationId }}</p>
      <p><strong>Guest Name:</strong> {{ reservation.guestName }}</p>
      <p><strong>Guest Phone Number:</strong> {{ reservation.guestPhoneNumber }}</p>
      <p><strong>Start Time:</strong> {{ reservation.reservationStart }}</p>
      <p><strong>End Time:</strong> {{ reservation.reservationEnd }}</p>
      <p><strong>Number of Guests:</strong> {{ reservation.numberOfGuests }}</p>
      <p><strong>Special Requests:</strong> {{ reservation.specialRequests || "None" }}</p>
    <div class="action-buttons">
        <button @click="editReservation" class="update-button">Edit Reservation</button>
        <button @click="deleteReservation" class="delete-button">Delete Reservation</button>
      </div>
    </div>
    <p v-else>Loading reservation details...</p>
    <div class="button-container">
      <button @click="goBack" class="back-button">Back to Reservations</button>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "ReservationDetails",
  props: ["tableId", "reservationId"],
  data() {
    return {
      reservation: null,
    };
  },
  methods: {
    editReservation() {
      this.$router.push({
        name: "ReservationEdit",
        params: { tableId: this.tableId, reservationId: this.reservationId },
      });
    },
    formatDateTime(dateTimeString) {
      const date = new Date(dateTimeString); 
      const formattedDate = date.toISOString().split('T')[0]; 
      const formattedTime = date.toTimeString().split(' ')[0];
      return `${formattedDate} ${formattedTime}`; 
    },
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
         this.reservation = {
                ...response.data,
                reservationStart: this.formatDateTime(response.data.reservationStart),
                reservationEnd: this.formatDateTime(response.data.reservationEnd),
            };
      } catch (error) {
        console.error("Failed to fetch reservation details:", error);
        alert("Error loading reservation details.");
      }
    },
    async deleteReservation() {
      if (!confirm("Are you sure you want to delete this reservation?")) return;

      try {
        const token = localStorage.getItem("token");
        await axios.delete(
          `https://restauranttablereservationsystem.azurewebsites.net/api/tables/${this.tableId}/reservations/${this.reservationId}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        alert("Reservation deleted successfully!");
        this.$router.push({ name: "TableReservations", params: { tableId: this.tableId } });
      } catch (error) {
        console.error("Failed to delete reservation:", error);
        alert("Error deleting reservation. Please try again.");
      }
    },
   goBack() {
  this.$router.push({
    name: "TableReservations",
    params: { tableId: this.tableId }, // Use the tableId prop to navigate
  });
   },
  },
  created() {
    this.fetchReservationDetails();
  },
};
</script>

<style>
.reservation-details {
  max-width: 600px;
  margin: 20px auto;
  padding: 20px;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  font-family: "Arial", sans-serif;
}

.reservation-details h1 {
  text-align: center;
  font-size: 1.8rem;
  color: #333;
  margin-bottom: 20px;
}

.details-card p {
  font-size: 1rem;
  margin: 10px 0;
  color: #555;
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
.action-buttons {
  display: flex;
  justify-content: space-around;
  margin-top: 20px;
}

.update-button {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.update-button:hover {
  background-color: #388e3c;
}

.delete-button {
  background-color: #f44336;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.delete-button:hover {
  background-color: #d32f2f;
}
</style>
