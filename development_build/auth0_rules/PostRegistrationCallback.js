function (user, context, callback) {
    // if it is the first login (hence the `signup`)
    if (context.stats.loginsCount > 1) {
      const axios = require('axios');
      var jwt = require('jsonwebtoken@7.1.9');
      var request = require('request@2.56.0');
      
      var userInfoToken = jwt.sign(
        {
          email: user.email,
          sub: user.user_id,
        },
        'MyClientSecret',
        {
          expiresIn: 4,
          audience: context.request.query.audience,
          issuer: 'https://' + context.request.hostname + '/'
        }
      );
      //console.log(userInfoToken);
      // construct the URL and POST body of our API request
      //https://penguinengine.azurewebsites.net/api/example
  
      const url = 'https://penguinengine.azurewebsites.net/api/user';
      const body = { Auth: user.user_id };
  
      // make our API call to add the user
      axios.post(url, body, {
          headers: {
              'content-type': 'application/json',
              Authorization: `Bearer ${userInfoToken}`    // send the access token through the 'Authorization' header
            }
        })
        .then(() => {
          // success!! update app_metadata with a flag to record this
          user.app_metadata.added_to_api = true;
          return auth0.users.updateAppMetadata(user.user_id, user.app_metadata);
        })
        .then(() => {
          // keep calm and carry on...
          callback(null, user, context);
        })
        .catch(() => {
          // ruh-roh! API request failed; record this in app_metadata
          user.app_metadata.added_to_api = false;
          return auth0.users.updateAppMetadata(user.user_id, user.app_metadata);
        })
        .then(() => {
          // keep calm and carry on...
          callback(null, user, context);
        });
    }
    callback(null, user, context);
  }