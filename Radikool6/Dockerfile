FROM microsoft/dotnet:sdk AS sdk
COPY . /src
WORKDIR /src
RUN dotnet publish -c release -o publish 

FROM microsoft/dotnet:aspnetcore-runtime
COPY --from=sdk /src/publish /Radikool6
WORKDIR /Radikool6
RUN mkdir /Radikool6/data && \
    apt-get update && \
    apt-get install ffmpeg -y
EXPOSE 5000
CMD ["dotnet", "Radikool6.dll"]