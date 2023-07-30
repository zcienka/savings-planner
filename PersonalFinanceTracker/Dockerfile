FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore "PersonalFinanceTracker.csproj"
COPY . .

FROM build AS publish
RUN dotnet publish "PersonalFinanceTracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

ENV ASPNETCORE_ENVIRONMENT=Development

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalFinanceTracker.dll"]