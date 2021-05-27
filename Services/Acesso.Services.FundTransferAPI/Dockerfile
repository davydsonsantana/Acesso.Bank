#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# healthcheck
RUN apt-get update
RUN apt-get install -y curl 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Acesso.Services.FundTransferAPI/Acesso.Services.FundTransferAPI.csproj", "Services/Acesso.Services.FundTransferAPI/"]
RUN dotnet restore "Services/Acesso.Services.FundTransferAPI/Acesso.Services.FundTransferAPI.csproj"
COPY . .
WORKDIR "/src/Services/Acesso.Services.FundTransferAPI"
RUN dotnet build "Acesso.Services.FundTransferAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Acesso.Services.FundTransferAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acesso.Services.FundTransferAPI.dll"]