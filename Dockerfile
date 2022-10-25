#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY VocareApi.sln ./
COPY Vocare/*.csproj ./Vocare/
COPY Vocare.Service/*.csproj ./Vocare.Service/
COPY Vocare.Data/*.csproj ./Vocare.Data/
COPY Vocare.Model/*.csproj ./Vocare.Model/
COPY Vocare.Util/*.csproj ./Vocare.Util/


RUN dotnet restore 
COPY . .

WORKDIR "/src/Vocare"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Vocare.Service"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Vocare.Data"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Vocare.Model"
RUN dotnet build -c Release -o /app

WORKDIR "/src/Vocare.Util"
RUN dotnet build -c Release -o /app


FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "Vocare.dll"]

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet Vocare.dll