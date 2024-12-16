FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-lib
WORKDIR /src

COPY . .

RUN dotnet restore ./ImageComparator/ImageComparator.csproj
RUN dotnet publish ./ImageComparator/ImageComparator.csproj -c Release -o /app/ImageComparator

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-cli
WORKDIR /src

COPY . .

RUN dotnet restore ./ImageCompareCLI/ImageCompareCLI.csproj
RUN dotnet publish ./ImageCompareCLI/ImageCompareCLI.csproj -c Release -o /app/ImageCompareCLI

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime

WORKDIR /app

RUN apt-get update && apt-get install -y libgdiplus

COPY --from=build-lib /app/ImageComparator /app/ImageComparator
COPY --from=build-cli /app/ImageCompareCLI /app/ImageCompareCLI

ENTRYPOINT ["dotnet", "ImageCompareCLI/ImageCompareCLI.dll"]
