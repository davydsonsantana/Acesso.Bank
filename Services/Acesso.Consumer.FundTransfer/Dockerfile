#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

# healthcheck
RUN apt-get update
RUN apt-get install -y curl 

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Consumer/Acesso.Consumer.FundTransfer/Acesso.Consumer.FundTransfer.csproj", "Consumer/Acesso.Consumer.FundTransfer/"]
RUN dotnet restore "Consumer/Acesso.Consumer.FundTransfer/Acesso.Consumer.FundTransfer.csproj"
COPY . .
WORKDIR "/src/Consumer/Acesso.Consumer.FundTransfer"
RUN dotnet build "Acesso.Consumer.FundTransfer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Acesso.Consumer.FundTransfer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acesso.Consumer.FundTransfer.dll"]