<template>
  <div class="row">
      <div class="col-12">
          <button class="btn btn-secondary mb-5" @click="decrement">{{ data.message }}</button>
          <div class="border border-primary">Result is: {{ data.count }}</div>
      </div>
  </div>
</template>

<script>
import SimpleStore from "../store/SimpleStore";

export default {
    name: "SecondComponent",
    computed: {
        data: () => SimpleStore.state,
    },
    watch: {
        data (newValue, oldValue) {
            if(newValue == 10) {
                console.log(`New value is ${newValue} old one was ${oldValue}`)
            }
        }
    },
    methods: {
        decrement() {
            SimpleStore.commit('decrement', {
                message: "This was triggered by secondary button"
            });
            console.log(SimpleStore.state.count);
        }
    }
}
</script>

<style>

</style>