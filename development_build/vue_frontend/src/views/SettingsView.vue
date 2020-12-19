<template>
  <div>
    <customer-settings />
    <suggestions />
  </div>
</template>

<script>
import CustomerSettings from "../components/CustomerSettings.vue";
import Suggestions from "../components/Suggestions.vue";

export default {
  components: { Suggestions, CustomerSettings },
};
</script>

<style>
</style>