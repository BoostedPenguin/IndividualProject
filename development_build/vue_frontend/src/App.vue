<template>
  <div>
    <NavigationBar />
    <SearchMenu />
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
import SearchMenu from "./components/SearchMenu";

export default {
  components: { NavigationBar, SearchMenu },
};
</script>