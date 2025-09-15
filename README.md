# SafeYard API 🛵🏞️

API RESTful para gerenciamento de motos, clientes e pátios, desenvolvida em .NET 9, utilizando Entity Framework Core com banco de dados Oracle. O sistema foi pensado para atuar como solução de controle e segurança em um pátio de motocicletas, permitindo o cadastro, consulta e gerenciamento dos veículos, seus proprietários e os pátios onde estão armazenados.

---

## ✅ Funcionalidades

- CRUD de Motos  
- CRUD de Clientes  
- CRUD de Pátios  
- Filtragem por QueryParams (ex: filtrar motos por marca ou ano mínimo)  
- Paginação nos endpoints de listagem  
- Integração com Banco de Dados Oracle via Entity Framework Core  
- Documentação automática da API via OpenAPI (Swagger)  

---

## 📌 Endpoints Principais

### 🛵 Motos

- `GET /api/motos` — Lista motos (filtros e paginação: `?marca=Honda&page=1&pageSize=10`)
- `GET /api/motos/{id}` — Retorna uma moto específica  
- `GET /api/motos/ano?minAno=2015` — Retorna motos com ano maior ou igual a `minAno`  
- `POST /api/motos` — Adiciona uma nova moto  
- `PUT /api/motos/{id}` — Atualiza uma moto existente  
- `DELETE /api/motos/{id}` — Remove uma moto  

### 👥 Clientes

- `GET /api/clientes` — Lista clientes (com paginação: `?page=1&pageSize=10`)
- `GET /api/clientes/{id}` — Retorna um cliente específico  
- `POST /api/clientes` — Cria um novo cliente  
- `PUT /api/clientes/{id}` — Atualiza um cliente  
- `DELETE /api/clientes/{id}` — Remove um cliente  

### 🏞️ Pátios

- `GET /api/patios` — Lista pátios (com paginação: `?page=1&pageSize=10`)
- `GET /api/patios/{id}` — Retorna um pátio específico  
- `POST /api/patios` — Adiciona um novo pátio  
- `PUT /api/patios/{id}` — Atualiza um pátio  
- `DELETE /api/patios/{id}` — Remove um pátio  

---

## 📋 Uso de Query Parameters e Paginação

Você pode usar parâmetros de consulta para filtrar e paginar os resultados:

- Filtrar motos por marca:  
  `GET /api/motos?marca=Honda`
- Listar motos com ano superior:  
  `GET /api/motos/ano?minAno=2018`
- Paginar resultados de qualquer entidade:  
  `GET /api/motos?page=2&pageSize=5`

**Exemplo de resposta paginada:**
```json
{
  "items": [
    {
      "id": 1,
      "modelo": "CG 160",
      "marca": "Honda",
      "ano": 2020,
      "clienteId": 3
    },
    ...
  ],
  "totalCount": 27,
  "page": 2,
  "pageSize": 5
}
```

---

## 📖 Documentação da API - Swagger (OpenAPI)

O projeto integra o Swagger para geração automática da documentação da API:

- Acesse `/swagger` após executar a aplicação para visualizar a documentação interativa  
- Permite testar os endpoints diretamente pelo navegador  
- Facilita o entendimento dos contratos da API (modelos, parâmetros, respostas)  

---

## 🧑‍💻 Exemplos de Uso (Payloads)

### Criar uma moto
**Request**
```json
POST /api/motos
{
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2020,
  "clienteId": 3
}
```
**Response**
```json
{
  "id": 10,
  "modelo": "CG 160",
  "marca": "Honda",
  "ano": 2020,
  "clienteId": 3
}
```

### Criar um cliente
**Request**
```json
POST /api/clientes
{
  "nome": "Carlos Andrade",
  "cpf": "12345678901",
  "email": "carlos@email.com"
}
```
**Response**
```json
{
  "id": 7,
  "nome": "Carlos Andrade",
  "cpf": "12345678901",
  "email": "carlos@email.com"
}
```

### Criar um pátio
**Request**
```json
POST /api/patios
{
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000",
  "capacidade": 200
}
```
**Response**
```json
{
  "id": 2,
  "nome": "Pátio Central",
  "endereco": "Av. Brasil, 1000",
  "capacidade": 200
}
```

---

## 🏛️ Justificativa Arquitetural

O SafeYard foi projetado utilizando o padrão de camadas, separando API, domínio e acesso a dados, o que facilita a escalabilidade, manutenção e testes automatizados. O .NET 9 foi escolhido pela maturidade, robustez e performance no desenvolvimento de APIs modernas. O Entity Framework Core proporciona integração eficiente e segura com o banco Oracle, abstraindo complexidades do acesso a dados e acelerando o desenvolvimento.

A decisão pelo Oracle se baseia na robustez, escalabilidade e recursos avançados oferecidos para aplicações corporativas. O uso do Swagger/OpenAPI reflete a preocupação com a transparência e usabilidade da API, facilitando a adoção e testes por terceiros. A implementação de paginação e filtros nos endpoints está alinhada com as melhores práticas de APIs REST, promovendo eficiência e flexibilidade nas consultas.

---

## 🗂️ Estrutura do Projeto

- **SafeYard.API**: Projeto da API REST (camada de apresentação)
- **SafeYard.Data**: Camada de acesso a dados com Entity Framework Core
- **SafeYard.Models**: Modelos de dados (Moto, Cliente, Patio)

---

## 👥 Equipe

- Adonay Rodrigues da Rocha  
- Pedro Henrique Martins Dos Reis  
- Thamires Ribeiro Cruz  

---

## 🚀 Como Executar

### Pré-requisitos

- .NET 9 SDK instalado  
- Oracle Database disponível e configurado  
- Visual Studio 2022 (ou VS Code)  

### Passos

1. **Clone o repositório:**
```bash
git clone https://github.com/AdonayRocha/SafeYard.git
cd SafeYard 
```
2. **Restaure os pacotes:**
```bash
dotnet restore
```
3. **Configure a conexão no appsettings.json:**
```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=SEU_SERVIDOR"
  }
}
```
4. **Aplique as migrations para criar as tabelas no banco:**
```bash
dotnet ef database update --project ./SafeYard.Data
```
5. **Execute a aplicação:**
```bash
cd SafeYard
dotnet run
```
6. **Acesse a documentação interativa:**
```
http://localhost:5000/swagger
```
