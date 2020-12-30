<template>
  <div>
    <admin-customer-support-overview v-if="isAdmin" />
    <customer-support-overview v-else-if="!isAdmin" />
    <suggestions />
  </div>
</template>

<script>
import { mapState } from "vuex";
import AdminCustomerSupportOverview from "../components/AdminCustomerSupportOverview.vue";
import CustomerSupportOverview from "../components/CustomerSupportOverview.vue";
import Suggestions from "../components/Suggestions.vue";

export default {
  components: {
    CustomerSupportOverview,
    Suggestions,
    AdminCustomerSupportOverview,
  },
  computed: mapState({
    isAdmin: (state) => state.user_role,
  }),
};
</script>

<style>
</style>