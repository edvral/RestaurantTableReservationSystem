import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import Restaurants from '../views/Restaurants.vue';
import RestaurantDetails from '../views/RestaurantDetails.vue';
import RestaurantTables from '../views/RestaurantTables.vue';
import TableReservations from '../views/TableReservations.vue';
import Register from "../views/Register.vue";
import Login from "../views/Login.vue";
import AddReservation from "../views/AddReservation.vue";
import ReservationDetails from "../views/ReservationDetails.vue";
import ReservationEdit from "../views/ReservationEdit.vue";
import Profile from "@/views/Profile.vue";
import AddRestaurant from "@/views/AddRestaurant.vue";
import EditRestaurant from "@/views/EditRestaurant.vue";
import AddTable from "@/views/AddTable.vue";
import EditTable from "@/views/EditTable.vue";

const routes = [
  { path: '/', name: 'Home', component: Home },
  { path: '/restaurants', name: 'Restaurants', component: Restaurants },
  { path: '/restaurants/:restaurantId', name: 'RestaurantDetails', component: RestaurantDetails, props: true }, 
  { path: '/restaurants/:restaurantId/tables', name: 'RestaurantTables', component: RestaurantTables, props: true },
  { path: '/tables/:tableId/reservations', name: 'TableReservations', component: TableReservations, props: true },
  { path: '/register', name: 'Register', component: Register},
  { path: '/login', name: 'Login', component: Login},  
  { path: '/tables/:tableId/add-reservation', name: 'AddReservation', component: AddReservation, props: true}, 
  { path: '/tables/:tableId/reservations/:reservationId', name: 'ReservationDetails', component: ReservationDetails, props: true},
  { path: '/tables/:tableId/reservations/:reservationId/edit', name: 'ReservationEdit', component: ReservationEdit, props: true}, 
  { path: '/profile', name: 'Profile', component: Profile, props: true},   
  { path: '/add-restaurant', name: 'AddRestaurant', component: AddRestaurant },  
  { path: '/restaurants/:restaurantId/edit', name: 'EditRestaurant', component: EditRestaurant },  
  { path: '/restaurants/:restaurantId/tables/add', name: 'AddTable', component: AddTable, props: true}, 
  { path: '/restaurants/:restaurantId/tables/:tableId/edit', name: 'EditTable', component: EditTable, props: true}, 
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");
  if (to.meta.requiresAuth && !token) {
    next("/login");
  } else {
    next();
  }
});

export default router;
