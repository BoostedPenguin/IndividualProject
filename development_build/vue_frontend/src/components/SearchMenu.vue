<template>
  <div>
    <div class="main-search">
      <div class="h-100 d-flex justify-content-center align-items-center">
        <div class="col-10 col-sm-8 col-md-6 col-lg-5 col-xl-4">
          <p class="text-center search-header outline">
            Choose a destination
          </p>
          <form>
            <div class="input-group mb-3">
              <div class="input-group-prepend">
                <button
                  class="btn filter-button "
                  data-toggle="dropdown"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  <i class="fa fa-caret-down" aria-hidden="true"></i>

                </button>

                <ul class="dropdown-menu checkbox-menu allow-focus">
                  <li>
                    <label @click.stop="stopTheEvent">
                      <input type="checkbox" /> Filter Item 1
                    </label>
                  </li>
                  <li>
                    <label @click.stop="stopTheEvent">
                      <input type="checkbox" /> Filter Item 2
                    </label>
                  </li>
                </ul>
              </div>
              <input
                type="text"
                class="form-control form-rounded"
                placeholder="Eiffel Tower, Paris"
                aria-label="Eiffel Tower, Paris"
                aria-describedby="basic-addon2"
              />

              <div class="input-group-append">
                <button class="btn btn-primary search-button" type="submit">
                  Search
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  methods: {
    stopTheEvent: (event) => event.stopPropagation(),
  },
};
</script>

<style>
.main-search {
  min-height: 600px;
  height: 1vh;
  background-image: url("../assets/main-background.png");
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
}

.form-rounded {
  border-radius: 1rem;
}

.filter-button {
  border-color: var(--secondary);
  background-color: var(--secondary-variant);
}

.search-button {
  border-top-right-radius: 1rem;
  border-bottom-right-radius: 1rem;
}


.filter-button:hover {
  background-color: #B55400;
}


.search-header {
  font-size: calc(15px + 1vw);
}

.outline {
    color: #fff;
    text-shadow: #000 0px 0px 5px;
    -webkit-font-smoothing: antialiased;
}

/* Dropdown checkbox styling */
.checkbox-menu li label {
  display: block;
  padding: 3px 10px;
  clear: both;
  font-weight: normal;
  line-height: 1.42857143;
  color: #333;
  white-space: nowrap;
  margin: 0;
  transition: background-color 0.4s ease;
}
.checkbox-menu li input {
  margin: 0px 5px;
  top: 2px;
  position: relative;
}

.checkbox-menu li.active label {
  background-color: #cbcbff;
  font-weight: bold;
}

.checkbox-menu li label:hover,
.checkbox-menu li label:focus {
  background-color: #f5f5f5;
}

.checkbox-menu li.active label:hover,
.checkbox-menu li.active label:focus {
  background-color: #b8b8ff;
}
</style>