FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . /app/
RUN dotnet restore 
RUN dotnet build --configuration Release -o /app/build
RUN dotnet publish --configuration Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish /app/
ENTRYPOINT ["dotnet","Expense Tracker.dll"]