FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ApplicationForm.sln ./
COPY src/ApplicationForm.Api/ApplicationForm.Api.csproj src/ApplicationForm.Api/
COPY src/ApplicationForm.Domain/ApplicationForm.Domain.csproj src/ApplicationForm.Domain/
COPY src/ApplicationForm.Infrastructure/ApplicationForm.Infrastructure.csproj src/ApplicationForm.Infrastructure/

RUN dotnet restore src/ApplicationForm.Api/ApplicationForm.Api.csproj

COPY src/ src/
RUN dotnet publish src/ApplicationForm.Api/ApplicationForm.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApplicationForm.Api.dll"]
