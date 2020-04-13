FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
RUN git clone https://github.com/llasapg/DiplomWork.git
WORKDIR /src/DiplomWork/DiplomaSolution
RUN dotnet restore "DiplomaSolution.csproj"
RUN dotnet build "DiplomaSolution.csproj" -c Release -o /app/build
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https --trust

FROM build AS publish
RUN dotnet publish "DiplomaSolution.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiplomaSolution.dll"]
