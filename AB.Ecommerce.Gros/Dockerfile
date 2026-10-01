FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AB.Ecommerce.Gros/AB.Ecommerce.Gros.csproj", "AB.Ecommerce.Gros/"]
COPY ["AB.Ecommerce.Gros.Application/AB.Ecommerce.Gros.Application.csproj", "AB.Ecommerce.Gros.Application/"]
COPY ["AB.Ecommerce.Gros.Shared/AB.Ecommerce.Gros.Shared.csproj", "AB.Ecommerce.Gros.Shared/"]
COPY ["AB.Ecommerce.Gros.Business/AB.Ecommerce.Gros.Business.csproj", "AB.Ecommerce.Gros.Business/"]
COPY ["AB.Ecommerce.Gros.Data/AB.Ecommerce.Gros.Data.csproj", "AB.Ecommerce.Gros.Data/"]
RUN dotnet restore "AB.Ecommerce.Gros/AB.Ecommerce.Gros.csproj"
COPY . .
WORKDIR "/src/AB.Ecommerce.Gros"
RUN dotnet build "AB.Ecommerce.Gros.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AB.Ecommerce.Gros.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AB.Ecommerce.Gros.dll"]