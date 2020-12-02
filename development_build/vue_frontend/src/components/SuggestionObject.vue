<template>
<transition name="bounce" v-if="show">
  <div class="card">
    <a class="stretched-link text-decoration-none" href="#">
      <img
        :src=photoReference
        class="card-img-top gallery-cover"
        alt="..."
      />
    </a>

    <div class="card-body">
      <h5 class="card-title">
        <div class="d-flex justify-content-between">
          <div>{{ placeLocation.name }}</div>
          <div></div>
        </div>
      </h5>
      <div class="card-text d-flex justify-content-between">
        <p class="out-of-stock" v-if="placeLocation.rating == null || placeLocation.rating == 0">
          Rating: unknown
        </p>
        <p class="out-of-stock" v-else>Rating: {{ placeLocation.rating }}</p>
      </div>
        <div>{{ placeLocation.vicinity }}</div>
    </div>
  </div>
</transition>
</template>

<script>

export default {
  data() {
    return {
      google_key: process.env.VUE_APP_GOOGLE_KEY,
      photoReference: "",
      show: false,
    };
  },
  mounted() {
    this.show = true;
    this.photoReference = `https://maps.googleapis.com/maps/api/place/photo?photoreference=${this.placeLocation.photoReference}&maxwidth=500&key=${this.google_key}`
  },
  props: ['placeLocation'],
  methods: 
  {
  },
};
</script>

<style>

.card:hover{
  transform: scale(1.05);
  box-shadow: 0 10px 20px rgba(0,0,0,.12), 0 4px 8px rgba(0,0,0,.06);
}

.bounce-enter-active {
  animation: bounce-in .5s;
}
.bounce-leave-active {
  animation: bounce-in .5s reverse;
}
@keyframes bounce-in {
  from {
    transform: scale(0.2);
  }
  to {
    transform: scale(1.0);
  }
}
</style>