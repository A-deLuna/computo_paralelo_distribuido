# Nota
Tal vez sea necesario agregar a su docker file:
```
RUN dotnet add package AWSSDK.S3
```

Un ejemplo de como se ve un dockerfile completo para los programas de asp net:
```
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
```

# Documentacion
https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/S3/TS3Client.html

# Para probar:
```
docker compose build
docker compose run aws
```
