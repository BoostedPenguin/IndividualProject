<template>
  <trip-content :tripId="tripId" />
</template>

<script>
import TripContent from "../components/TripContent.vue";
export default {
  components: { TripContent },
  props: ["tripId"],
};
</script>

<style>
</style>