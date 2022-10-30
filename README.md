## Context
This RESTFUL API enables business owners and public institutions to setup a virtual waiting queues for their customers.
## Database
Worked with a NO SQL database. Used Mongodb Atlas to provision a mongodb cluster and access it from the API.
## Security
Worked with Token Based Authentication and Role Based Authorization. (Jwt Bearer Tokens)
## Design Patterns
This project uses repository pattern allowing it to be flexible in terms of the type of database chosen and abstract the database access in the business logic.
Worked with the built in DI tools offered by Microsoft.
## Testing and hosting
Used postman for testing, used collection variables and code snippets to automate token collection and assignment to collection variables.
Used Docker to containerize the web service and host it on Heroku.
## Notes
Make sure to adjust the connections string in the startup file (Or program file if sdk>5) to your local or cloud based Mongodb cluster. 
A full report is written on this project you can contact me to obtain things like UML diagrams and more in deepth technical overview of the project.
