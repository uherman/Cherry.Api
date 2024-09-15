# Description: Build and push the docker image and deploy the proxy service to the k3s cluster

dotnet publish -c Release

# Build and push the docker image
docker build . -t uherman/cherry-proxy
docker push uherman/cherry-proxy

# Deploy the proxy service to the k3s cluster
k3s apply -f deployment.yml
k3s rollout restart deployment cherry-proxy