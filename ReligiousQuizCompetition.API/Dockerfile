FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ReligiousQuizCompetition.API/ReligiousQuizCompetition.API.csproj"
WORKDIR "/src/ReligiousQuizCompetition.API"
RUN dotnet publish "ReligiousQuizCompetition.API.csproj" -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "ReligiousQuizCompetition.API.dll"]
