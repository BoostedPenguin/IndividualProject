<template>
  <div>
    <view-ticket :ticketid="ticketid" />
    <suggestions />
  </div>
</template>

<script>
import Suggestions from "../components/Suggestions.vue";
import ViewTicket from "../components/ViewTicket.vue";
export default {
  components: { ViewTicket, Suggestions },
  props: ["ticketid"],
};
</script>

<style>
</style>