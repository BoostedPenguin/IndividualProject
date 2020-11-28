<template>
  <div class="main">
    <div class="container-fluid">
      <div class="row mt-5">


        <div class="col-xl-3 col-lg-4 col-md-4 col-sm-12 col-12 pt-3" v-for="index in 8" :key="index">

          <div class="card">
            <a class="stretched-link text-decoration-none" href="#">
              <img
                src="../assets/EiffelTower.jpg"
                class="card-img-top  gallery-cover"
                alt="..."
              />
            </a>
            
            <div class="card-body">
              <h5 class="card-title">
                <div class="d-flex justify-content-between">
                  <div>Name</div>
                  <div>
                    <p class="out-of-stock">Rating?</p>
                  </div>
                </div>
              </h5>
              <div class="card-text d-flex justify-content-between">
                <div>Description</div>
              </div>
            </div>
            
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style>
body {
  background-color: var(--primary-variant);
}

/* Card body control  */
.gallery-cover {
  width: 100%;
  min-height: 100px;
  height: 20vw;
  object-fit: cover;
}

@media (max-width: 768px) {
  .gallery-cover {
    height: 60vw;
  }
}

.card {
    border: 1px;
      box-shadow: 1px 2px 7px 6px rgba(0,0,0,0.25);


  transition: 0.5s;
}

.container-fluid {
  padding-right: 50px;
  padding-left: 50px;
  margin-right: auto;
  margin-left: auto;
}
</style>