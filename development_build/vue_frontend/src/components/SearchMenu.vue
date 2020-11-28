<template>
  <div>
    <div class="main-search">
      <div class="h-100 d-flex justify-content-center align-items-center">
        <div class="col-10 col-sm-8 col-md-6 col-lg-5 col-xl-4">
          <p class="text-center search-header">
            Some text specifying some other text
          </p>
          <form>
            <div class="input-group mb-3">
              <input
                type="text"
                class="form-control form-rounded"
                placeholder="Eiffel Tower, Paris"
                aria-label="Eiffel Tower, Paris"
                aria-describedby="basic-addon2"
              />
              <div class="input-group-append">
                <button
                  class="btn btn-primary dropdown-toggle"
                  data-toggle="dropdown"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  Filter
                </button>

                <ul class="dropdown-menu checkbox-menu allow-focus">
                  <li>
                    <label  @click.stop="stopTheEvent"> <input type="checkbox"  /> Filter Item 1 </label>
                  </li>
                  <li>
                    <label  @click.stop="stopTheEvent"> <input type="checkbox" /> Filter Item 2 </label>
                  </li>
                </ul>
              </div>
            </div>
            <div class="col-6 offset-3">
              <button class="btn btn-primary btn-block" type="submit">
                Search
              </button>
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
        stopTheEvent: (event) => event.stopPropagation() 
    }
};
</script>

<style>
.main-search {
  min-height: 400px;
  height: 1vh;
  background-image: url("../assets/main-background.png");
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
}

.form-rounded {
  border-radius: 1rem;
}

.search-header {
  font-size: calc(12px + 1vw);
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
    margin:0;
    transition: background-color .4s ease;
}
.checkbox-menu li input {
    margin: 0px 5px;
    top: 2px;
    position: relative;
}

.checkbox-menu li.active label {
    background-color: #cbcbff;
    font-weight:bold;
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