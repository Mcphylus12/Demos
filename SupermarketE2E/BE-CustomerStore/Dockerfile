FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["BE-CustomerStore/BE-CustomerStore.csproj", "BE-CustomerStore/"]
RUN dotnet restore "BE-CustomerStore/BE-CustomerStore.csproj"
COPY ["BE-CustomerStore/**", "BE-CustomerStore"]

WORKDIR "/src/BE-CustomerStore"
RUN dotnet publish "BE-CustomerStore.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BE-CustomerStore.dll"]