<template>
  <div>
    <my-account />
    <suggestions />
  </div>
</template>

<script>
import MyAccount from "../components/MyAccount";
import Suggestions from "../components/Suggestions.vue";

export default {
  components: {
    MyAccount,
    Suggestions,
  },
};
</script>

<style>
</style>