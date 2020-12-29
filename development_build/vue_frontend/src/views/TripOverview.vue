<template>
  <div>
    <trip-content :tripId="tripId" />
    <suggestions />
  </div>
</template>

<script>
import Suggestions from "../components/Suggestions.vue";
import TripContent from "../components/TripContent.vue";
export default {
  components: { TripContent, Suggestions },
  props: ["tripId"],
};
</script>

<style>
</style>