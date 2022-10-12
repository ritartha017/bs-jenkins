FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build-image

WORKDIR /app

COPY ./*.sln ./
COPY ./src/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

COPY tests/*/*.csproj ./  
RUN for file in $(ls *.csproj); do mkdir -p tests/${file%.*}/ && mv $file tests/${file%.*}/; done

RUN dotnet restore

COPY . .
RUN dotnet test ./tests/Endava.BookSharing.Application.UnitTests/Endava.BookSharing.Application.UnitTests.csproj

RUN dotnet publish ./src/Endava.BookSharing.Presentation/Endava.BookSharing.Presentation.csproj -o /publish/

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal

WORKDIR /publish

COPY --from=build-image /publish .

ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

ENTRYPOINT ["dotnet", "Endava.BookSharing.Presentation.dll"]
