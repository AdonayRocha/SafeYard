# SafeYard API 🛵🏞️

API RESTful para gerenciamento de motos, clientes e pátios, desenvolvida em .NET 9, utilizando Entity Framework Core com banco de dados Oracle.

---

## ✅ Funcionalidades

- CRUD de Motos  
- CRUD de Clientes  
- CRUD de Pátios  
- Filtragem por QueryParams para algumas consultas (ex: filtrar motos por marca ou ano mínimo)  
- Integração com Banco de Dados Oracle via Entity Framework Core  
- Documentação automática da API via OpenAPI (Swagger)  

---

## 📌 Endpoints Principais

### 🛵 Motos

- `GET /api/motos` — Retorna todas as motos; aceita filtro opcional por marca: `GET /api/motos?marca=Honda`  
- `GET /api/motos/{id}` — Retorna uma moto específica  
- `GET /api/motos/ano?minAno=2015` — Retorna motos com ano maior ou igual a `minAno`  
- `POST /api/motos` — Adiciona uma nova moto  
- `PUT /api/motos/{id}` — Atualiza uma moto existente  
- `DELETE /api/motos/{id}` — Remove uma moto  

### 👥 Clientes

- `GET /api/clientes` — Retorna todos os clientes  
- `GET /api/clientes/{id}` — Retorna um cliente específico  
- `POST /api/clientes` — Cria um novo cliente  
- `PUT /api/clientes/{id}` — Atualiza um cliente  
- `DELETE /api/clientes/{id}` — Remove um cliente  

### 🏞️ Pátios

- `GET /api/patios` — Retorna todos os pátios  
- `GET /api/patios/{id}` — Retorna um pátio específico  
- `POST /api/patios` — Adiciona um novo pátio  
- `PUT /api/patios/{id}` — Atualiza um pátio  
- `DELETE /api/patios/{id}` — Remove um pátio  

---

## 📋 Uso de Query Parameters

Alguns endpoints permitem filtragem via query parameters para consultas dinâmicas, por exemplo:

- `GET /api/motos?marca=Honda` — filtra motos pela marca  
- `GET /api/motos/ano?minAno=2018` — retorna motos com ano maior ou igual ao valor passado  

Isso permite que os clientes da API façam buscas específicas sem necessidade de criar muitos endpoints separados.

---

## 📖 Documentação da API - Swagger (OpenAPI)

O projeto integra o Swagger para geração automática da documentação da API:

- Acesse `/swagger` após executar a aplicação para visualizar a documentação interativa  
- Permite testar os endpoints diretamente pelo navegador  
- Facilita o entendimento dos contratos da API (modelos, parâmetros, respostas)  

---

🗂️ Estrutura do Projeto
SafeYard.API: Projeto da API REST
SafeYard.Data: Camada de acesso a dados com Entity Framework Core
SafeYard.Models: Modelos de dados (Moto, Cliente, Patio)

---
## Equipe

Adonay Rodrigues da Rocha
Pedro Henrique Martins Dos Reis
Thamires Ribeiro Cruz

---

## 🚀 Como Executar

### Pré-requisitos

- .NET 9 SDK instalado  
- Oracle Database disponível e configurado  
- Visual Studio 2022 (ou VS Code)  

### Passos

1. **Clone o repositório:**

```bash
git clone https://github.com/seu-usuario/SafeYard.git
cd SafeYard 
```


2. **Restaure os pacotes:**

```bash
dotnet restore
```

3. **Configure a conexão com o banco no appsettings.json:**

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

5. **Execute a aplicação: **

```bash
cd SafeYard
dotnet run
```
