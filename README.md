# Product Control API

Este projeto é uma API REST para controle de produtos, movimentação de estoque e gerenciamento de usuários, construída com .NET 9 e PostgreSQL. O sistema utiliza práticas de DDD (Domain-Driven Design) e segue a arquitetura Clean Architecture, autenticação JWT e conteinerização com Docker.

## 📁 Estrutura de Pastas e Objetivos

O projeto está dividido em camadas para garantir a separação de responsabilidades:

- **ProductControl.Domain**: O coração da aplicação. Contém as entidades, interfaces de repositório, serviços de domínio e ValueObjects. É independente de frameworks externos.
- **ProductControl.Application**: Camada de orquestração. Contém os DTOs (Data Transfer Objects), interfaces de serviços de aplicação e suas implementações, lidando com o fluxo de dados entre a API e o Domínio.
- **ProductControl.Infrastructure**: Implementações técnicas. Contém o contexto do banco de dados (`PostgreDbContext`), migrações do EF Core, repositórios e serviços externos (como integração com ViaCep e criptografia de senhas).
- **Source (Web API)**: A porta de entrada do sistema. Contém os Controllers, configurações de dependência (`Program.cs`) e configurações de ambiente.
- **ProductControl.Tests**: Testes unitários e de integração utilizando xUnit e Moq para garantir a qualidade e o funcionamento das regras de negócio.

---

## 🔒 Segurança e Autenticação

### Armazenamento de Senhas
O sistema prioriza a segurança dos dados. As senhas dos usuários nunca são armazenadas em texto simples. Elas são processadas usando o padrão **Hash + Salt** (Rfc2898/PBKDF2 com 100.000 iterações), garantindo proteção contra ataques de dicionário e rainbow tables.

### Autenticação JWT e Roles
A autenticação é feita via **JWT (JSON Web Token)**. Ao realizar o login, o usuário recebe um token que deve ser enviado no cabeçalho das requisições subsequentes. O acesso aos recursos é controlado por **Roles** (Papéis), garantindo que cada usuário acesse apenas o que lhe é permitido.

## 📊 Matriz de Permissionamento

| Controller | Endpoint | Administrator | User |
| :--- | :--- | :---: | :---: |
| **Authentication** | Login | ✅ | ✅ |
| | Logout | ✅ | ✅ |
| **Products** | Listar Todos / Buscar por ID | ✅ | ✅ |
| | Criar Produto | ✅ | ✅ |
| | Atualizar Dados do Produto | ✅ | ✅ |
| | Adicionar/Remover Estoque | ✅ | ✅ |
| | Ativar/Desativar Produto | ✅ | ❌ |
| **Users** | Criar Novo Usuário | ✅ | ❌ |
| | Listar Todos / Buscar por ID | ✅ | ❌ |
| | Atualizar Dados de Qualquer Usuário | ✅ | ❌ |
| | Atualizar Meus Próprios Dados | ✅ | ✅ |
| | Ativar/Desativar Usuário | ✅ | ❌ |
| **StockMovement** | Ver Todas Movimentações | ✅ | ❌ |
| | Ver Movimentações por Produto/Usuário | ✅ | ❌ |

---

## 🐳 Execução via Docker Compose

O sistema está totalmente configurado para rodar em containers. A **Key do JWT** e os dados de conexão do **Banco de Dados** são injetados via variáveis de ambiente no arquivo `docker-compose.yml`, mantendo as credenciais sensíveis fora do código-fonte.

### Como executar:

1.  Certifique-se de ter o Docker e o Docker Compose instalados.
2.  Na raiz do projeto (onde está o arquivo `docker-compose.yml`), execute:
    ```bash
    docker-compose up --build
    ```
3.  A API estará disponível em `http://localhost:5000/swagger/index.html` e o pgAdmin (para gerenciar o banco) em `http://localhost:5050/login?next=/browser/`: E-mail: admin@admin.com | Senha: admin.

**Nota**: Ao iniciar pela primeira vez, o sistema criará automaticamente um usuário administrador padrão (UserName: `admin` /Password: `Admin!123`) através do serviço de semente (seed).

## 🧪 Como Rodar os Testes

Para garantir que as alterações não quebrem funcionalidades existentes, execute os testes unitários:

```bash
dotnet test
```
---

## 🚀 Swagger e Autenticação

Para testar os endpoints e entender a documentação:

1. Acesse http://localhost:5000/swagger/index.html (em ambiente de desenvolvimento).
2. Para autenticar:
  - Vá ao endpoint POST /api/authentication/login.
  - Use as credenciais do administrador (admin) ou de um usuário criado.
  - Copie o token retornado no campo token.
  - No topo da página do Swagger, clique no botão Authorize.
  - Digite o token no campo de valor (formato: seu_token_aqui) e clique em Authorize.
3. Agora todos os endpoints protegidos estarão liberados para teste com base no seu nível de permissão.
