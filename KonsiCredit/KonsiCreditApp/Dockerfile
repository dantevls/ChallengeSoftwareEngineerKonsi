# Imagem base para construir o projeto Blazor
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia o arquivo de projeto e restaura as dependências
COPY . ./
RUN dotnet restore

# Copia o restante dos arquivos do projeto e compila
COPY . .
RUN dotnet publish -c Release -o out

# Imagem base para executar a aplicação publicada
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# Define o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copia os arquivos publicados do estágio anterior
COPY --from=build /app/out ./

# Expõe a porta 80 para acesso HTTP
EXPOSE 80

# Define o comando de entrada para iniciar a aplicação
ENTRYPOINT ["dotnet", "KonsiCreditApp.dll"]
