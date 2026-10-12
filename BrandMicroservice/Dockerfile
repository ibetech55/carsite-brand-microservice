FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /app

COPY . .

RUN dotnet publish BrandMicroservice.csproj -o /build

FROM mcr.microsoft.com/dotnet/aspnet:10.0

RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /build .

EXPOSE 5106

ENTRYPOINT ["dotnet", "BrandMicroservice.dll", "--urls", "http://0.0.0.0:5106"]