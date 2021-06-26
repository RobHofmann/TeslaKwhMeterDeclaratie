# Buildtime Container
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY src/TeslaKwhMeter/TeslaKwhMeter.csproj .
RUN dotnet restore TeslaKwhMeter.csproj

COPY src/TeslaKwhMeter/ .
RUN dotnet publish -c Release -o out

# Runtime Container Image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
LABEL Author="Rob Hofmann <rob.hofmann@me.com>"

# Add Cron
RUN apt-get -q update && \
    apt-get -qy dist-upgrade && \
    apt-get install -y --no-install-recommends cron && \
    apt-get -y autoremove && \
    apt-get -y clean && \
    rm -rf /var/lib/apt/lists/*

# Make sure we setup the entrypoint script
COPY ./entrypoint.sh /entrypoint.sh
RUN chmod u+x /entrypoint.sh

# Place the app binaries
WORKDIR /app
COPY --from=build /app/out ./

# Define the Entrypoint
ENTRYPOINT ["/entrypoint.sh"]