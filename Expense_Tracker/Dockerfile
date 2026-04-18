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
ENV ASPNETCORE_URLS=http://+:80 \
    ConnectionStrings__DevConnection= \
    SqlServer__Host= \
    SqlServer__Port=1433 \
    SqlServer__Database= \
    SqlServer__User= \
    SqlServer__Password= \
    SqlServer__Encrypt=false \
    SqlServer__TrustServerCertificate=true \
    SqlServer__MultipleActiveResultSets=false \
    SqlServer__ConnectionTimeout=30
RUN addgroup --gid $GID appgroup && \
    adduser --system --uid $UID --ingroup appgroup appuser    
COPY --from=build /app/publish /app/
EXPOSE 80
RUN chown -R appuser:appgroup /app
USER appuser
ENTRYPOINT ["dotnet","Expense Tracker.dll"]
