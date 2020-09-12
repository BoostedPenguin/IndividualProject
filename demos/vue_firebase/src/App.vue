<template>
  <div>
    <main class="py-1">
    <Navbar />
    <router-view class="mt-5"></router-view>
    </main>
  </div>
</template>

<script>
import Navbar from '../src/components/navbar/Navbar'

export default {
  components: {
    Navbar
  }
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
