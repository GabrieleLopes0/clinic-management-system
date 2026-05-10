# Clinic Management System

Aplicação backend para gestão de agendamentos de consultas em clínica fictícia.

## Tecnologias

- ASP.NET Core 10
- Dapper
- JWT Authentication
- PostgreSQL
- Swagger
- Serilog
- xUnit
- Docker

## O que foi feito

- Backend consolidado em um único projeto `Clinic.Api`
- CRUD de pacientes
- CRUD de profissionais
- Agendamento de consultas com regras de negócio:
  - paciente só pode ter 1 consulta por profissional por dia
  - profissional não pode atender duas consultas no mesmo horário
  - horário de atendimento de 08:00 a 18:00, segunda a sexta
  - consultas de 30 minutos
- Autenticação JWT
- Swagger para documentação
- Logs gravados em arquivo com Serilog
- Testes unitários para validação das regras de agendamento
- Docker para API e PostgreSQL

## Como rodar localmente

1. Instale Docker Desktop.
2. Execute no diretório do projeto:

```bash
docker compose up --build
```

3. A API ficará disponível em:

- `http://localhost:5000`
- `http://localhost:5000/swagger/index.html`

## Como rodar o frontend

1. Abra outro terminal e navegue até a pasta `frontend`:

```bash
cd frontend
```

2. Instale as dependências:

```bash
npm install
```

3. Inicie a aplicação frontend:

```bash
npm run dev
```

4. Acesse o frontend em:

- `http://localhost:5173`

> O frontend consome a API em `http://localhost:5000`.

## Banco de dados

O container PostgreSQL inicializa com o script em `Clinic.Api/Scripts/init.sql`.

O banco é criado com:

- usuário: `postgres`
- senha: `postgres`
- banco: `clinicdb`

## Usuário de teste

- email: `admin@clinic.com`
- senha: `123456`

## Endpoints principais

### Autenticação

- `POST /api/auth/login`

### Pacientes

- `GET /api/patients`
- `POST /api/patients`
- `GET /api/patients/{id}`
- `PUT /api/patients/{id}`
- `DELETE /api/patients/{id}`

### Profissionais

- `GET /api/professionals`
- `POST /api/professionals`
- `GET /api/professionals/{id}`

### Consultas

- `POST /api/appointments`
- `GET /api/appointments/professional/{id}`

> Todos os endpoints além do login exigem o token JWT no header `Authorization: Bearer {token}`.

## Como testar

```bash
dotnet test Clinic.Tests/Clinic.Tests.csproj
```

## Decisões técnicas

- Usei Dapper para acesso leve ao banco e maior controle de SQL.
- PostgreSQL no Docker atende o requisito de banco real.
- JWT garante autenticação simples e compatível com frontend.
- Serilog grava logs em arquivo e facilita a análise de problemas.
- A arquitetura foi simplificada em um único projeto backend com pastas claras.

## Próximos passos

- frontend básico em HTML/JS ou React
- refresh token
- mais testes de integração
- melhorias de tratamento de erros e responses
