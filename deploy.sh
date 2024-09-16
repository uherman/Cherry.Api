# Description: Build and push the docker image and deploy the api to the k3s cluster

dotnet publish -c Release

# Build and push the docker image
docker build . -t uherman/cherry-api
docker push uherman/cherry-api

# Replace tokens in the deployment.yml file
export $(grep -v '^#' .env | xargs)
envsubst < deploy/deployment.yml > deploy/.secret.deployment.yml

# Deploy the api to the k3s cluster
k3s apply -f deploy/.secret.deployment.yml
k3s rollout restart deployment cherry-api

# Clean up
rm deploy/.secret.deployment.yml