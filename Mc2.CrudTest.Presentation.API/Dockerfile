FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Mc2.CrudTest.Presentation.API/Mc2.CrudTest.Presentation.API.csproj", "Mc2.CrudTest.Presentation.API/"]
COPY ["Mc2.CrudTest.Application/Mc2.CrudTest.Application.csproj", "Mc2.CrudTest.Application/"]
COPY ["Mc2.CrudTest.Domain/Mc2.CrudTest.Domain.csproj", "Mc2.CrudTest.Domain/"]
COPY ["Mc2.CrudTest.Infrastructure/Mc2.CrudTest.Infrastructure.csproj", "Mc2.CrudTest.Infrastructure/"]
RUN dotnet restore "Mc2.CrudTest.Presentation.API/Mc2.CrudTest.Presentation.API.csproj"
COPY . .
WORKDIR "/src/Mc2.CrudTest.Presentation.API"
RUN dotnet build "Mc2.CrudTest.Presentation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mc2.CrudTest.Presentation.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mc2.CrudTest.Presentation.API.dll"]