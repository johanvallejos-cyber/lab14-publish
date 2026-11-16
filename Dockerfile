FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TuApp.Domain/TuApp.Domain.csproj", "TuApp.Domain/"]
COPY ["TuApp.Application/TuApp.Application.csproj", "TuApp.Application/"]
COPY ["TuApp.Infrastructure/TuApp.Infrastructure.csproj", "TuApp.Infrastructure/"]
COPY ["lab11-VallejosJohan/lab11-VallejosJohan.csproj", "lab11-VallejosJohan/"]

RUN dotnet restore "lab11-VallejosJohan/lab11-VallejosJohan.csproj"

COPY . .
WORKDIR /src/lab11-VallejosJohan
RUN dotnet publish "lab11-VallejosJohan.csproj" --configuration Release --no-restore --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 10000
CMD ["sh", "-c", "dotnet lab11-VallejosJohan.dll --urls http://0.0.0.0:${PORT:-10000}"]
