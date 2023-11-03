FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src

COPY ["Demo/DemoWorker/DemoWorker.csproj", "Demo/DemoWorker/"]
COPY ["Demo/DemoCore/DemoCore.csproj", "Demo/DemoCore/"]
COPY ["Libs/Communication/Communication.csproj", "Libs/Communication/"]
COPY ["Libs/Communication.Abstractions/Communication.Abstractions.csproj", "Libs/Communication.Abstractions/"]

#RUN dotnet restore "Demo/DemoWorker/DemoWorker.csproj"

COPY ["Demo/DemoWorker/**", "Demo/DemoWorker"]
COPY ["Demo/DemoCore/**", "Demo/DemoCore"]
COPY ["Libs/Communication/**", "Libs/Communication"]
COPY ["Libs/Communication.Abstractions/**", "Libs/Communication.Abstractions"]

WORKDIR "Demo/DemoWorker"
RUN dotnet publish "DemoWorker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWorker.dll"]