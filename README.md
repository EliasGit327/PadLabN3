1. `docker-compose up -d --scale mongo=4 --scale microservice=N` where N is number of instances of the microservice
2. `docker-compose exec mongo mongo`
3. Paste the content of `initiateReplicaSet.js` file