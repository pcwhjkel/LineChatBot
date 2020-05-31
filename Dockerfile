FROM mcr.microsoft.com/dotnet/core/sdk:3.1.200-bionic
MAINTAINER 林昀龍
COPY . /var/www
WORKDIR /var/www
EXPOSE 5001
RUN dotnet build
ENTRYPOINT ["dotnet", "run"]