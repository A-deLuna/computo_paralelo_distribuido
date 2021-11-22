FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
COPY App/ App/
WORKDIR App
RUN dotnet build
#CMD dotnet add package AWSSDK.S3
CMD ["dotnet", "bin/Debug/net5.0/App.dll"]
