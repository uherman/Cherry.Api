# Description: Build and push the docker image and deploy the api to the k3s cluster

dotnet publish -c Release

# Build and push the docker image
docker build . -t uherman/cherry-api
docker push uherman/cherry-api

# Deploy the api to the k3s cluster
k3s apply -f deploy/deployment.yml
k3s rollout restart deployment cherry-api