import { reactive } from 'vue';

export const authState = reactive({
    isLoggedIn: !!localStorage.getItem("token"), 
    username: localStorage.getItem("username") || "", 
});
