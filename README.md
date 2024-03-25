API de Busca de Matrícula de Servidor

Esta API foi desenvolvida para buscar e retornar a matrícula de um servidor em uma API externa. É parte do desafio proposto pela Konsi para coletar e processar dados de servidores aposentados ou pensionistas.

Requisitos:

Antes de rodar a API em um ambiente Docker usando Docker Compose, certifique-se de ter instalado o seguinte:

Docker | 
.NET 6

Como Rodar o Projeto com Docker Compose:

Siga os passos abaixo para rodar a API em um ambiente Docker usando Docker Compose:

1. Clonar o Repositório
Clone este repositório em seu ambiente de desenvolvimento:

git clone https://github.com/dantevls/Software-Engineer-Konsi.git

cd KonsiCredit

2. Rodar o Docker Compose:

Depois de configurar o arquivo docker-compose.yml, execute o seguinte comando na raiz do projeto para iniciar os contêineres definidos no arquivo:

docker-compose up -d

3. Acessar o APP:

Após rodar o Docker Compose, a API estará acessível em http://localhost:8080. Você pode usar ferramentas como CURL, Postman ou seu navegador para fazer requisições à API.

Parar e Remover os Contêineres:

Para parar e remover os contêineres Docker gerenciados pelo Docker Compose, execute o seguinte comando na raiz do projeto:

docker-compose down
