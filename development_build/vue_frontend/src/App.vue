<template>
  <div>
    <NavigationBar />
  </div>
</template>

<style>
#app {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>

<script>
import NavigationBar from "./components/NavigationBar";

export default {
  components: { NavigationBar },
};
</script>