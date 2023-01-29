# Define base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
WORKDIR /source
COPY . .
RUN dotnet restore "./API/PkAPI.csproj"
RUN dotnet publish "./API/PkAPI.csproj" -c Release -o /publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY API/ /app
COPY --from=builder /publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "PkAPI.dll"]
