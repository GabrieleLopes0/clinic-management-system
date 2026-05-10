FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Clinic.Api/Clinic.Api.csproj Clinic.Api/
RUN dotnet restore Clinic.Api/Clinic.Api.csproj

COPY . .
WORKDIR /src/Clinic.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Clinic.Api.dll"]
