function (user, context, callback) {
  // if it is the first login (hence the `signup`)
  if (context.stats.loginsCount === 1) {


    const request = require("request");
    const axios = require('axios');
    const url = 'https://penguinengine.azurewebsites.net/api/user';
    const innerBody = { Auth: user.user_id };

    var options = {
      method: 'POST',
      url: 'https://dev-3sn-ksqj.eu.auth0.com/oauth/token',
      headers: { 'content-type': 'application/json' },
      body: 'SECRET'
    };

    request(options, function (error, response, body) {
      if (error) throw new Error(error);

      let parsedData = JSON.parse(body);

      // make our API call to add the user
      axios.post(url, innerBody, {
        headers: {
          'content-type': 'application/json',
          Authorization: `Bearer ${parsedData.access_token}`    // send the access token through the 'Authorization' header
        }
      })
        .then(() => {
          // keep calm and carry on...
          callback(null, user, context);
        })
        .catch((err) => {
          console.log(err);
        });
    });
  }
  callback(null, user, context);
}

