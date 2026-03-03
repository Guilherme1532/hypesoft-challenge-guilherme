FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Hypesoft.sln ./
COPY Hypesoft.API/Hypesoft.API.csproj Hypesoft.API/
COPY Hypesoft.Application/Hypesoft.Application.csproj Hypesoft.Application/
COPY Hypesoft.Infrastructure/Hypesoft.Infrastructure.csproj Hypesoft.Infrastructure/
COPY Hypesoft.Domain/Hypesoft.Domain.csproj Hypesoft.Domain/

RUN dotnet restore Hypesoft.API/Hypesoft.API.csproj

COPY . .
RUN dotnet publish Hypesoft.API/Hypesoft.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "Hypesoft.API.dll"]
