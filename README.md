![](https://i.imgur.com/qc9okwt.png)

***
# Important
The project has been completed successfully. Hosting the whole project isn't an option at the moment due to increasing expenses for Google API. You can still host it locally if you provide Google Maps with the following APIs: Directions, Distance Matrix, Geocoding, Maps Javascript, Places. Additionally, you would need to provide an IPStack access token.

***
# The vision behind the product:
Have you ever tried planning for a trip? You have to take into account the destination, the time spent traveling, other nearby sightseeing options. You spend too much time sightseeing and oops, you don't have time to visit your next destination and have to return back to your hotel.

Here is where PenguinEngine kicks in to save the day. We do all the legwork so you can enjoy your vacation in peace without worrying that you may miss out on something!

***

# What do we offer:
* Trip planning, time management and recommendations based on **your** preferences - **all in one package**!
* Want to plan ahead? That's ok! We support **multiple trips**.
* **Advanced suggestions algorithm** so you can have the most breathtaking experience on your trip!
* See what **destinations are trending** with other users of PenguinEngine!

***
# Mockups
If you want to see all of the mockups for the website please visit: https://github.com/BoostedPenguin/PenguinEngine/tree/master/UXDesign

# Back-end
For a back-end language I had to choose something that had support / SDKs for API Endpoints, WebRequests, Auth0. The choice was **.NET Core Web API**.
Unlike NET Framework, NET Core isn't exclusive to machines that run only on Windows, but can be used on different OS such as Linux / macOS.

Another option for the project was Java with Spring. However, after deep examination of the differences between Java and NET Core - I chose the latter, because it has: Built-in middleware, Security, JSON Optimization and most importantly - easier access to Azure services such as SQL Server database.

# Front-end
There were 3 candidates for a front-end framework: Angular, Vue, React. The choice was **Vue**.
From the start Angular was removed from the consideration list because of the steep learning curve compared to the other two, has significantly slower performance and is mostly used for big projects and big teams. If the project backend was NodeJS with Express, the obvious choice for a front-end framework would be React, so the whole project was all JavaScript. However, since that wasn't the case I chose Vue because it is a full-fledged framework without JSX with far-better separation of concerns.

# Authentication
Usually the authentication is created in the backend, however since our focus is to create a distributed system I decided to use a third-party service called **Auth0** that would do the legwork for me. It handles the user authentication and only shares a JWT Token with the client for security reasons. Integrating the Auth0 service with the back-end and front-end was done with the provided SDKs.

# Data Persistence
The database is an Azure hosted **SQL Server**. It's a relational-database, which I believe should be better at handling the volume of data, which the website will produce. NoSQL was in consideration, however as for the moment the project won't store that much information but it's always an option when it comes to scalability.

# Testing
For my project I had to make sure both the back-end and front-end are tested that's why I decided to use integration tests for NET Core (XUnit with DI), I have tested all important services for the essential work of my back-end. 
For the front-end I use Cypress E2E Testing on vue.js
