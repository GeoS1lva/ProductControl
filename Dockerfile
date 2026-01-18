FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Source/Source.csproj", "Source/"]
COPY ["ProductControl.Application/ProductControl.Application.csproj", "ProductControl.Application/"]
COPY ["ProductControl.Domain/ProductControl.Domain.csproj", "ProductControl.Domain/"]
COPY ["ProductControl.Infrastructure/ProductControl.Infrastructure.csproj", "ProductControl.Infrastructure/"]

RUN dotnet restore "Source/Source.csproj"

COPY . .
WORKDIR "/src/Source"
RUN dotnet build "Source.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Source.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Source.dll"]