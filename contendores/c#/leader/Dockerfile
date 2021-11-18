FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY MyApi MyApi
WORKDIR MyApi
RUN dotnet build
ENTRYPOINT dotnet /MyApi/bin/Debug/net5.0/MyApi.dll --urls=http://+:8080
