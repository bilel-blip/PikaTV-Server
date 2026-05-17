FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["pikatv.csproj", "."]
RUN dotnet restore "./pikatv.csproj"
COPY . .
RUN dotnet publish "pikatv.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "pikatv.dll"]