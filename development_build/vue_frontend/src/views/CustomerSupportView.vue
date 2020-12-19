<template>
  <div>
    <customer-support-overview />
    <suggestions />
  </div>
</template>

<script>
import CustomerSupportOverview from "../components/CustomerSupportOverview.vue";
import Suggestions from "../components/Suggestions.vue";

export default {
  components: { CustomerSupportOverview, Suggestions },
};
</script>

<style>
</style>