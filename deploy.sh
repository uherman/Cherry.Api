# Description: Build and push the docker image and deploy the api to the k3s cluster

# Build and push the docker image
docker build . -t uherman/cherry-api
docker push uherman/cherry-api

# Deploy the api to the k3s cluster
k3s apply -f deploy/deployment.yml