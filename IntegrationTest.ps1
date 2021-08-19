docker-compose -f docker-compose.integration.yml build
docker-compose -f docker-compose.integration.yml up --abort-on-container-exit
