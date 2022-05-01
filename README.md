# StockChatApp
This project is designed to test your knowledge of back-end web technologies, specifically in .NET and assess your ability to create back-end products with attention to details, standards, and reusability. 

AUTHOR - Uche Igbokwe

Application was built using clean architecture, SOLID principles.

## DEPENDENCIES/PREREQUISITIES

* .NET 6.0
* .NET MVC Framework
* Pusher Service
* Http Client Factory
* RabbitMq
* XUnit
* Ensure port 5002, 5003 and 5004 are available

## SETUP

> **Note**: Begin by cloning the project and navigate into the StockChatApp folder. Ensure to have .NET 6, running on your device. 

# SERVICES:
To startup the SERVICES project, follow these steps:

* Navigate to the [API](src/Services/) project folder
  * `cd src/Services`
  * `docker compose up`
* Run the command below and listen on http://localhost:5003/swagger/index.html and http://localhost:5004/swagger/index.html
  > **Note**: No need to visit the swagger page.

To run the test, follow these steps:

* Navigate to the [Tests](src/Services/Stocks/Tests/) project folder
  * `cd src/Services/Stocks/Tests`
  * `dotnet build`
  * `dotnet test`

* Navigate to the [Tests](src/Services/Chat/Tests/) project folder
  * `cd src/Services/Chat/Tests`
  * `dotnet build`
  * `dotnet test`

# CLIENT:
To startup the CLIENT project, follow these steps:

* Navigate to the [client](src/Client/) project folder
  * `cd src/Client/`
* Run the code below to install dependencies.
  * `dotnet restore`  
* Startup the CLIENT project
  * `dotnet start`  

* You will be prompted to register or login on the landing page.
  * `http://localhost:5002/`
* When you have completed registration and log in, navigate to the link below.
  * `http://localhost:5002/chat`  




## HOW THE APPLICATION WORKS
The Project is designed to allow user to chat in a group and also request for stock details by typing in a special command `/stock=aapl.us`


- Create a group and add users to the group

- Select the group that was created, then begin to chat.


- Exception handlers are setup to manage too many requests, unauthorized and generic error responses from the API.


Incase of blockers, kindly contact me via: uchehenryigbokwe@gmail.com.
