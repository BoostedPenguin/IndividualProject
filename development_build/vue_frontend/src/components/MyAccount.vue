<template>
  <div class="main-content">
    <div class="container">
      <div class="row h-100 align-items-center text-center">
        <div class="col-lg-4 col-12 mt-3">
          <div class="col-lg-12 offset-lg-0 col-8 offset-2">
            <router-link to="/" class="text-decoration-none">
              <div class="card myaccount-card">
                <div class="row justify-content-center">
                  <div class="col-10">
                    <img
                      class="card-img-top img-fluid"
                      src="../assets/user.png"
                      alt="Card image cap"
                    />
                  </div>
                </div>

                <div class="card-body">
                  <p class="card-text">Information</p>
                </div>
              </div>
            </router-link>
          </div>
          <hr class="hr-black" />
        </div>
        <div class="col-lg-4 col-12 mt-3">
          <div class="col-lg-12 offset-lg-0 col-8 offset-2">
            <router-link to="/support" class="text-decoration-none">
              <div class="card myaccount-card">
                <div class="row justify-content-center">
                  <div class="col-10">
                    <img
                      class="card-img-top img-fluid"
                      src="../assets/customer_support.png"
                      alt="Card image cap"
                    />
                  </div>
                </div>
                <div class="card-body">
                  <p class="card-text">Support</p>
                </div>
              </div>
            </router-link>
          </div>
          <hr class="hr-black" />
        </div>

        <div class="col-lg-4 col-12 mt-3 mb-3 mb-lg-0">
          <div class="col-lg-12 offset-lg-0 col-8 offset-2">
            <router-link to="/" class="text-decoration-none">
              <div class="card myaccount-card">
                <div class="row justify-content-center">
                  <div class="col-10">
                    <img
                      class="card-img-top img-fluid"
                      src="../assets/settings.png"
                      alt="Card image cap"
                    />
                  </div>
                </div>
                <div class="card-body">
                  <p class="card-text">Settings</p>
                </div>
              </div>
            </router-link>
          </div>
          <hr class="hr-black" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      error: "",
    };
  },
  methods: {},
};
</script>

<style scoped>
.main-content {
  min-height: 600px;
  display: flex;
  background-color: rgb(255, 255, 255);
}

.myaccount-card {
  border-radius: 1.5rem;
  border: 1px solid black;
  -webkit-box-shadow: 0px 6px 18px 8px rgba(0, 0, 0, 0.38);
  box-shadow: 0px 6px 18px 8px rgba(0, 0, 0, 0.15);

  font-size: calc(10px + 1vw);

  background-size: 100% 200%;
  background-image: linear-gradient(
    to bottom,
    #fff 50%,
    var(--secondary-variant) 50%
  );
  transition: 0.5s;
}
.myaccount-card:hover,
.myaccount-card:focus {
  background-position: 0 100%;
  color: #fff;
  transform: scale(1.1);
}

.hr-black {
  background-color: black;
}
</style>