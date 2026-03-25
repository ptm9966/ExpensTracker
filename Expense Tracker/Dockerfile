FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY *.csproj /app/
COPY . /app/
RUN dotnet restore 

RUN dotnet publish --configuration Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
ARG UID=1001
ARG GID=1001
RUN addgroup --gid $GID appgroup && \
    adduser --system --uid $UID --ingroup appgroup appuser    
COPY --from=build /app/publish /app/
EXPOSE 8080
RUN chown -R appuser:appgroup /app
USER appuser
ENTRYPOINT ["dotnet","Expense Tracker.dll"]