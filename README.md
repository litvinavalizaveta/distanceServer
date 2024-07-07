# distanceServer
REST server to measure distance in miles between two airports.
To run the server:
1. Run Docker
2. Open the project folder in terminal and type 'docker compose up -d' command
The server will be running on port 50422. 
Example GET request: http://localhost:50422/api/distance?from=MSQ&to=LAX
