<template>
  <div class="home">
    <div class="hero">
      <h1 class="system-name">Restaurant Table Reservation System</h1>
      <p class="tagline">Your convenient way to reserve tables at top restaurants</p>
    </div>
    <div class="image-container">
      <img
        src="@/assets/restaurant.jpg"
        alt="Example Restaurant"
        class="restaurant-image"
      />
    </div>
  </div>
</template>

<script>
export default {
  name: "AppHome",
};
</script>

<style>
.home {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
  padding: 20px;
  font-family: 'Roboto', sans-serif;
}

.hero {
  margin-bottom: 20px;
}

.system-name {
  font-size: 3rem; 
  font-weight: bold;
  color: black;
  margin: 20px 0;
}

.tagline {
  font-size: 1.2rem;
  color: black;
}

.image-container {
  max-width: 100%;
  overflow: hidden;
}

.restaurant-image {
  max-width: 45%;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}
</style>
