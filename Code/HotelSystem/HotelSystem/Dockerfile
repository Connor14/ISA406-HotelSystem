FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HotelSystem/HotelSystem.csproj", "HotelSystem/"]
RUN dotnet restore "HotelSystem/HotelSystem.csproj"
COPY . .
WORKDIR "/src/HotelSystem"
RUN dotnet build "HotelSystem.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HotelSystem.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HotelSystem.dll"]