<template>
  <div class="row">
    <div class="col-12 mb-5">
      <button class="btn btn-primary" @click="increment">{{ data.message }}</button>
    </div>
  </div>
</template>

<script>
import SimpleStore from "../store/SimpleStore"

export default {
  name: 'HelloWorld',
  props: {
    msg: String
  },
  computed: {
    data: () => SimpleStore.state 
  },
  methods: {
    increment() {
      SimpleStore.commit('increment', {
        message: "This was triggered by primary button",
      })
      console.log(SimpleStore.state.count)
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
