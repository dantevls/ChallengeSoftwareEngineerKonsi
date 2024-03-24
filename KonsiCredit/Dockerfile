# Use the .NET 6 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
  
  # Copy the .csproj and restore dependencies
  
  # Copy the rest of the application code and build
COPY . ./
RUN dotnet restore 
RUN dotnet publish ./Presentation/Presentation.KonsiCredit -c Release -o out
RUN dotnet publish ./Presentation/Consumer.KonsiCredit -c Release -o out 

  
  # Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
  
  # Expose port 5015
EXPOSE 5015
