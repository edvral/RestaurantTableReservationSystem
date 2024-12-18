<template>
  <div class="table-reservations">
    <h1>Reservations for Table {{ tableId }}</h1>
    <div v-if="reservations.length" class="reservation-list">
      <div v-for="reservation in reservations" :key="reservation.reservationId" class="reservation-card">
        <p><strong>Start Time:</strong> {{ reservation.reservationStart }}</p>
        <p><strong>End Time:</strong> {{ reservation.reservationEnd }}</p>
         <button
          v-if="isLoggedIn && (isAdmin || reservation.userId === currentUserId)"
          @click="viewDetails(reservation.tableId, reservation.reservationId)"
          class="view-details-button"
        >
          View Details
        </button>
      </div>
    </div>
    <p v-else>No reservations for this table.</p>
      <div v-if="isLoggedIn" class="add-reservation-container">
      <button @click="addReservation" class="add-reservation-button">Add Reservation</button>
    </div>
    <div class="button-container">
      <button @click="goBack" class="back-button">Back to Tables</button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { authState } from "../state/authState";

export default {
  name: "TableReservations",
  props: ["tableId"],
  data() {
    return {
      restaurantId: localStorage.getItem("currentRestaurantId"),
      reservations: [], 
      isLoggedIn: authState.isLoggedIn,
      currentUserId: parseInt(localStorage.getItem("userId")),
      isAdmin: localStorage.getItem("username") === "admin",
    };
  },
  methods: {
    formatDateTime(dateTimeString) {
      const date = new Date(dateTimeString); 
      const formattedDate = date.toISOString().split('T')[0]; 
      const formattedTime = date.toTimeString().split(' ')[0];
      return `${formattedDate} ${formattedTime}`; 
    },
    goBack() {
  const restaurantId = localStorage.getItem("currentRestaurantId");
  if (restaurantId) {
    this.$router.push({ path: `/restaurants/${restaurantId}/tables` });
  } else {
    this.$router.push({ path: `/restaurants` }); // Fallback
  }
},
    addReservation() {
      this.$router.push({ name: "AddReservation", params: { tableId: this.tableId } });
    },
    viewDetails(tableId, reservationId) {
      this.$router.push({ name: "ReservationDetails", params: { tableId, reservationId } });
    },
  },
  created() {
   axios
      .get(`https://restauranttablereservationsystem.azurewebsites.net/api/tables/${this.tableId}/reservations`)
      .then((response) => {
        this.reservations = response.data.map((reservation) => {
          return {
            ...reservation,
            reservationStart: this.formatDateTime(reservation.reservationStart),
            reservationEnd: this.formatDateTime(reservation.reservationEnd),
          };
        });
      })
      .catch((error) =>
        console.error(`Error fetching reservations: ${error}`)
      );
  },
};
</script>

<style>
.table-reservations {
  max-width: 800px;
  margin: 20px auto;
  padding: 20px;
  font-family: 'Arial', sans-serif;
  border: 1px solid #ddd;
  border-radius: 10px;
  background-color: #f9f9f9;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.table-reservations h1 {
  font-size: 1.8rem;
  color: black;
  text-align: center;
  margin-bottom: 20px;
}

.reservation-list {
  margin-top: 20px;
}

.reservation-card {
  border: 1px solid #ddd;
  padding: 10px;
  margin-bottom: 10px;
  border-radius: 5px;
  background-color: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: box-shadow 0.3s ease, transform 0.3s ease; 
  position: relative;
}

.reservation-card:hover {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); 
  transform: translateY(-5px);
}

.reservation-card p {
  margin: 5px 0;
  font-size: 1rem;
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
.add-reservation-container {
  text-align: center;
  margin-top: 20px;
}

.add-reservation-button {
  background-color: #4caf50;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s ease;
}

.add-reservation-button:hover {
  background-color: #388e3c; 
}
.view-details-button {
  position: absolute; 
  background-color: #007bff;
  color: white;
  border: none;
  padding: 8px 15px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 0.9rem;
  margin-top: 10px;
  transition: background-color 0.3s ease;
  right: 10px; 
  top: 35%; 
  transform: translateY(-50%); 
}

.view-details-button:hover {
  background-color: #0056b3;
}
</style>
