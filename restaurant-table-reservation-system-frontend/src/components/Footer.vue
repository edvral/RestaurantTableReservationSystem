<template>
  <footer>
    <div class="footer-content">
      <div class="social-media">
        <a href="#" target="_blank" aria-label="Facebook">
          <i class="fab fa-facebook"></i>
        </a>
        <a href="#" target="_blank" aria-label="Twitter">
          <i class="fab fa-twitter"></i>
        </a>
        <a href="#" target="_blank" aria-label="Instagram">
          <i class="fab fa-instagram"></i>
        </a>
      </div>
      <div class="footer-links">
        <router-link to="/terms">Terms of Service</router-link>
        <router-link to="/privacy">Privacy Policy</router-link>
        <router-link to="/contact">Contact Us</router-link>
      </div>
      <p class="copyright">
        © {{ new Date().getFullYear() }} Restaurant Table Reservation System. All rights reserved.
      </p>
    </div>
  </footer>
</template>

<script>
export default {
  name: "AppFooter",
};
</script>

<style>
footer {
  background-color: #3f51b5;
  color: white;
  text-align: center;
  padding: 20px 10px;
  margin-top: auto;
}
.footer-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 10px;
}
.social-media{
  margin-bottom: 15px;
}
.social-media a {
  color: white;
  margin: 0 10px;
  font-size: 1.5rem;
  text-decoration: none;
}
.footer-links {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 15px;
  margin-bottom: 5px;
}
.footer-links a {
  color: white;
  text-decoration: none;
}
.footer-links a:hover {
  text-decoration: underline;
}
.social-media a:hover {
  color: #ffcc00;
}
@media (max-width: 768px) {
  .footer-links {
    flex-direction: column;
    gap: 5px;
  }
}
</style>
