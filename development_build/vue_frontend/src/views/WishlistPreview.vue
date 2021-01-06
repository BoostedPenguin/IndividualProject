<template>
  <div>
    <wishlist-set-origin-destination />
    <suggestions />
  </div>
</template>

<script>
import Suggestions from "../components/Suggestions.vue";
import WishlistSetOriginDestination from "../components/WishlistSetOriginDestination.vue";
export default {
  components: { WishlistSetOriginDestination, Suggestions },
};
</script>

<style>
</style>