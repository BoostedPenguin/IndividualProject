<template>
  <div>
    <NavigationBar />
    <Home />
  </div>
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>

<script>
import NavigationBar from "./components/NavigationBar";
import Home from "./views/Home";

export default {
  components: { NavigationBar, Home },
};
</script>