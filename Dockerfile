FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src

COPY IntegrationTestPractice/IntegrationTestPractice.csproj /src/IntegrationTestPractice/
COPY IntegrationTestPractice.Tests/IntegrationTestPractice.Tests.csproj /src/IntegrationTestPractice.Tests/
COPY IntegrationTestPractice.sln /src/

RUN dotnet restore

COPY . .

RUN dotnet build -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ARG env

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "NCARB.ePortfolio.API.dll"]