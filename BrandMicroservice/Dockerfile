FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . .

RUN dotnet publish BrandMicroservice.csproj -o /build

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /build .

ENTRYPOINT ["dotnet", "BrandMicroservice.dll", "--urls", "http://0.0.0.0:5106"]