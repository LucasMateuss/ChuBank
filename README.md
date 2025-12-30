# ChuBank API



API de integração para gerenciamento de contas e transferências bancárias.



A aplicação segue os princípios de **Clean Architecture** e utiliza **Docker** para orquestração do ambiente.



## Tecnologias Utilizadas



- **.NET 8** 

- **Entity Framework Core**

- **SQL Server 2022** 

- **Docker & Docker Compose** 

- **xUnit & Moq**

- **BrasilAPI** 

- **Swagger**

- **JWT Bearer**

- **FluentValidation**

- **MemoryCache**


## Requisitos



- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e rodando.

- Git.



## Como Executar o Projeto



A aplicação foi desenhada para rodar inteiramente via Docker, não sendo necessária a instalação do SDK do .NET nem do SQL Server na máquina local.



1. **Clone o repositório:**

  ```bash

  git clone https://github.com/LucasMateuss/ChuBank

  cd ChuBank

  ```



2. **Suba o ambiente (API + Banco de Dados):**

  Na raiz do projeto (onde está o arquivo docker-compose.yml), execute:
   ```bash

  docker-compose up --build

  ```

3. **Acesse a Documentação (Swagger):**: 

  - Acesse: http://localhost:8081/swagger

4. **Guia de Uso (Autenticação):**

   A API é protegida. Siga este fluxo para testar as funcionalidades:

   - **Criar Conta:**  
     Use o endpoint `POST /api/v1/Accounts` (informe uma senha).

   - **Login:**  
     Use o endpoint `POST /api/v1/Auth/Login` com os dados criados.

   - **Copiar Token:**  
     Copie o token JWT gerado na resposta.

   - **Autorizar:**
     - No Swagger, clique no cadeado **Authorize**
     - Cole o token no campo de texto  
       (o sistema já adiciona o prefixo `Bearer` automaticamente)
     - Clique em **Authorize**

   - **Testar:**  
     Agora você pode acessar rotas protegidas como **Transferências** e **Extratos**.



## Como Rodar os Testes

**Para rodar os testes, execute na raiz do projeto:**
```bash

dotnet test

```


## Estrutura da Solução

A solução está dividida em camadas para separar responsabilidades:



- **ChuBank.Domain**: O coração do sistema. Contém as Entidades (Account, Transaction), Interfaces de Repositório e Serviços de Domínio. Não depende de nenhuma outra camada externa.

- **ChuBank.Application**: Contém os Serviços de Aplicação (TransferService), DTOs e a orquestração das regras de negócio (ex: verificar feriado antes de transferir).

- **ChuBank.Infrastructure**: Implementação técnica. Contém o DbContext (EF Core), Repositórios, Cliente HTTP da BrasilAPI e implementação de Cache.

- **ChuBank.Api**: Entrada da aplicação. Contém os Controllers, configuração de Injeção de Dependência e Dockerfile.

- **ChuBank.Tests**: Testes automatizados utilizando xUnit e Moq.






**Desenvolvido por Lucas Mateus de Souza.**

