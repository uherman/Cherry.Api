FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY Proxy/bin/Release/net8.0 .

# Create a group and user so we are not running our container and application as root
RUN addgroup --group appuser --gid 2000 && adduser --uid 1000 --gid 2000 appuser 

# Serve on port 5000, we cannot serve on port 80 with a custom user that is not root.
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

# Set the environment variable to trim the memory usage of the container
ENV MALLOC_TRIM_THRESHOLD_=200000

# Run as new user
RUN chown appuser:appuser /app
USER 1000 

ENTRYPOINT ["dotnet", "Proxy.dll"]
