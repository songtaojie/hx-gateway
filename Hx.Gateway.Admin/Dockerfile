#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Hx.Gateway.Admin/Hx.Gateway.Admin.csproj", "Hx.Gateway.Admin/"]
COPY ["Hx.Gateway.Core/Hx.Gateway.Core.csproj", "Hx.Gateway.Core/"]
RUN dotnet restore "Hx.Gateway.Admin/Hx.Gateway.Admin.csproj"
COPY . .
WORKDIR "/src/Hx.Gateway.Admin"
RUN dotnet build "Hx.Gateway.Admin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hx.Gateway.Admin.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# 设置时区为上海
ENV TZ=Asia/Shanghai
# 设置时区信息
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT ["dotnet", "Hx.Gateway.Admin.dll"]