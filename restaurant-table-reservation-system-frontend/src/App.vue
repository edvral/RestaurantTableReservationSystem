<template>
  <div id="app">
    <header>
      <Navbar />
    </header>
    <main>
      <router-view />
    </main>
    <footer>
      <Footer />
    </footer>
  </div>
</template>

<script>
import Navbar from './components/Navbar.vue';
import Footer from './components/Footer.vue';

export default {
  components: {
    Navbar,
    Footer,
  },
};
</script>

<style>
#app {
  font-family: 'Roboto', sans-serif;
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
header, footer {
  background-color: #3f51b5;
  color: white;
  text-align: center;
  padding: 10px 0;
}
main {
  flex: 1;
  padding: 20px;
}
</style>
