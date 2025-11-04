# 🏍️ SafeYard API

API RESTful para **controle e segurança de pátios de motocicletas**, desenvolvida em **.NET 9** com **Entity Framework Core** e **Oracle Database**.  
O **SafeYard** permite o registro de motos, proprietários e pátios, oferecendo **endpoints CRUD, filtros, paginação e documentação via Swagger**.

---

## 💡 Visão do Domínio — Por que este projeto existe

Para administração de  **pátios de motocicletas** (como estacionamentos ou concessionárias) que enfrentam desafios de controle:  
- Perda ou duplicação de registros;  
- Falta de rastreabilidade de quem é o proprietário;  
- Dificuldade para saber onde cada moto está alocada.

O **SafeYard** foi criado para resolver esses problemas, fornecendo um sistema centralizado e seguro de **cadastro, alocação e consulta** de motos, clientes e pátios.  

Com ele, é possível:  
- Registrar e consultar **motos e proprietários**;  
- Controlar a **localização atual** de cada moto (qual pátio está alocada);  
- Monitorar **capacidade e ocupação dos pátios**;  
- Expor dados via API para **integrações externas e painéis administrativos**.

---

## 🧩 Entidades Principais e Relacionamentos

| Entidade | Atributos | Relacionamentos |
|-----------|------------|----------------|
| **Cliente** | `Id`, `Nome`, `CPF` (único), `Email`, `Telefone`, `DataCriacao` | 1 Cliente → N Motos |
| **Moto** | `Id`, `Modelo`, `Marca`, `Ano`, `Placa` (única), `ClienteId` (FK), `PatioId` (FK, opcional), `DataCadastro` | Moto → 1 Cliente<br>Moto → 0..1 Pátio |
| **Pátio** | `Id`, `Nome`, `Endereco`, `Capacidade`, `DataCriacao` | 1 Pátio → N Motos |

> **Importante:**  
> As entidades devem conter **propriedades de navegação** (ex.: `public Cliente Cliente { get; set; }`) e as **chaves estrangeiras configuradas** no `DbContext`.  
> Isso garante o correto funcionamento dos relacionamentos no **Entity Framework Core**, permitindo o uso de `Include`, `cascade delete`, validações de integridade e queries otimizadas.

---

## ⚙️ Funcionalidades Principais

- CRUD completo de **Motos**, **Clientes** e **Pátios**  
- Filtros e paginação nas listagens (`marca`, `ano`, `clienteId`, etc.)  
- Validações de unicidade (`CPF`, `Placa`) e obrigatoriedade de campos  
- Documentação interativa via **Swagger / OpenAPI**  
- Integração com **Oracle Database**  
- Arquitetura em camadas (.API, .Data, .Models) com separação de responsabilidades  

---

## 🚀 Endpoints Principais

**Prefixo base:** `/api`

### Motos
- `GET /api/motos` — Listagem com filtros e paginação  
- `GET /api/motos/{id}`  
- `POST /api/motos`  
- `PUT /api/motos/{id}`  
- `DELETE /api/motos/{id}`  

### Clientes
- `GET /api/clientes`  
- `GET /api/clientes/{id}`  
- `POST /api/clientes`  
- `PUT /api/clientes/{id}`  
- `DELETE /api/clientes/{id}`  

### Pátios
- `GET /api/patios`  
- `GET /api/patios/{id}`  
- `POST /api/patios`  
- `PUT /api/patios/{id}`  
- `DELETE /api/patios/{id}`  

**Códigos de resposta esperados:** `200`, `201 (Created)`, `204`, `400`, `404`, `422`, `500`.

---

## 🧾 Exemplos de Requisições

### Criar Moto (POST `/api/motos`)
**Request**
```json
{
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2020,
  "placa": "ABC1D23",
  "clienteId": 3,
  "patioId": 1
}
```

**Response (201 Created)**
```json
{
  "id": 10,
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2020,
  "placa": "ABC1D23",
  "clienteId": 3,
  "patioId": 1,
  "dataCadastro": "2025-11-04T00:00:00Z"
}
```

### Exemplo de Listagem Paginada
```json
{
  "items": [ /* motos */ ],
  "totalCount": 27,
  "page": 2,
  "pageSize": 5,
  "totalPages": 6
}
```

---

## 📘 Swagger e Autenticação

A documentação interativa está disponível em:  
**`http://localhost:{porta}/swagger`**

Para fins de **avaliação e testes locais**, use as credenciais padrão:  
- **Usuário:** `admin`  
- **Senha:** `admin`

---

## 🧪 Testes Automatizados (`dotnet test`)

O projeto **SafeYard.Tests** contém testes de unidade e integração escritos com **xUnit**.  
Para executá-los, basta rodar na raiz da solução:

```bash
dotnet test
```

Durante a execução, o .NET:

- Compila todos os projetos de teste.
- Localiza métodos marcados com `[Fact]` e `[Theory]`.
- Executa os testes automaticamente, gerando relatórios de sucesso/falha.
- Ignora os testes com o atributo `[Fact(Skip = "motivo")]`.

### 🧩 Testes Atuais no Projeto

Atualmente, o repositório inclui os seguintes testes:

| Classe | Método | Tipo | Descrição |
|--------|---------|------|-----------|
| `ApiKeyMiddlewareTests` | `InvokeAsync_ApiKeyInvalida_Retorna401` | Unidade | Verifica se o middleware retorna **401 Unauthorized** quando a API Key é inválida. |
| `MotoControllerTests` | `PostMoto_ModeloVazio_RetornaBadRequest` | Unidade | Garante que o endpoint `/motos` retorna **400 Bad Request** quando o modelo é inválido. |
| `ClientesControllerTests` | `DeleteCliente_ClienteNaoExiste_RetornaNotFound` | Unidade | Testa o comportamento ao tentar excluir um cliente inexistente (**404 Not Found**). |
| `HealthCheckIntegrationTests` | `HealthEndpoint_Retorna200` | Integração | Sobe uma instância da API via `WebApplicationFactory` e verifica se `/health` responde **200 OK**. |
| `UnitTest1` | `Test1` | Unidade | Exemplo genérico de teste base para o setup inicial. |

---

## 🛠️ Como Executar Localmente

### Pré-requisitos
- .NET 9 SDK  
- Oracle Database (local, container ou remoto)  
- `dotnet-ef` (opcional, para migrations)  

### Passos
```bash
# 1. Clonar repositório
git clone https://github.com/AdonayRocha/SafeYard.git
cd SafeYard

# 2. Restaurar dependências
dotnet restore

# 3. Configurar a connection string (em appsettings.json)
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.host:1521/ORCL"
  }
}

# 4. Aplicar migrations
dotnet ef database update --project ./SafeYard.Data --startup-project ./SafeYard.API

# 5. Executar a API
dotnet run --project ./SafeYard.API
```

Abra o navegador em **http://localhost:{porta}/swagger** para acessar a documentação.

---

## 🧱 Estrutura Esperada do Repositório

```
SafeYard/
├── SafeYard.sln
├── src/
│   ├── SafeYard.API/
│   ├── SafeYard.Data/
│   ├── SafeYard.Models/     
│   └── SafeYard.Application/ 
└── tests/
    ├── SafeYard.UnitTests/
    └── SafeYard.IntegrationTests/
```

---

## 👥 Equipe

- **Adonay Rodrigues da Rocha**  
- **Pedro Henrique Martins dos Reis**  
- **Thamires Ribeiro Cruz**

---

## ✅ Observações Finais para Avaliação

- As entidades **Moto**, **Cliente** e **Pátio** devem possuir **propriedades de navegação** e **chaves estrangeiras configuradas**.  
- Verifique se há **migrations válidas** e que o projeto **compila corretamente**.  
- O README contém todas as instruções para execução e testes, inclusive as credenciais de avaliação (`admin` / `admin`).
- Para testes via Swagger utilize (`admin`)
