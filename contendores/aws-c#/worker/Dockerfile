FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY MyApi/MyApi.csproj MyApi/
COPY MyApi/obj/ MyApi/obj/
COPY MyApi/appsettings.Development.json MyApi/
COPY MyApi/appsettings.json MyApi/

WORKDIR MyApi
RUN dotnet add package AWSSDK.S3
COPY MyApi/*.cs .
COPY MyApi/Controllers/ ./Controllers/
RUN dotnet build
ENTRYPOINT dotnet /MyApi/bin/Debug/net5.0/MyApi.dll --urls=http://+:8080
