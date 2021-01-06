<template>
  <div class="main">
    <SearchMenu />
    <Suggestions />
  </div>
</template>

<script>
import SearchMenu from "../components/SearchMenu";
import Suggestions from "../components/Suggestions";

export default {
  components: {
    SearchMenu,
    Suggestions,
  },
};
</script>

<style>
</style>