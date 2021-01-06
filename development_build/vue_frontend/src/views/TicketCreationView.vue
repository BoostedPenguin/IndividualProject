<template>
  <div>
    <create-ticket />
    <suggestions />
  </div>
</template>

<script>
import CreateTicket from "../components/CreateTicket.vue";
import Suggestions from "../components/Suggestions.vue";
export default {
  components: { CreateTicket, Suggestions },
};
</script>

<style>
</style>