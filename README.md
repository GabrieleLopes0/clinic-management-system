# Clinic Management System

Sistema para gestão de agendamentos de consultas em clínica médica. Permite cadastrar pacientes, profissionais e agendar consultas com regras de negócio específicas.

## Tecnologias

- **Back-end**: C# com .NET 10 (Web API) e Dapper com PostgreSQL
- **Front-end**: React com Vite
- **Banco**: PostgreSQL em Docker
- **Autenticação**: JWT
- **Documentação**: Swagger
- **Logs**: Serilog
- **Testes**: xUnit

## Como rodar

### Pré-requisitos
- Docker Desktop

### Instalação
1. Clone o repositório:
```bash
git clone <https://github.com/GabrieleLopes0/clinic-management-system.git>
cd clinic-management-system
```

### Executando com Docker
Na raiz do projeto, execute:
```bash
npm run start
```
Ou diretamente:
```bash
docker compose up --build
```

Depois acesse:
- **Frontend**: http://localhost:5173
- **API (Swagger)**: http://localhost:5000/swagger

### Desenvolvimento local (opcional)
Se preferir desenvolver sem Docker:

1. Instale PostgreSQL localmente
2. Configure a connection string em `Clinic.Api/appsettings.Development.json`
3. Execute o backend:
```bash
cd Clinic.Api
dotnet run
```
4. Execute o frontend:
```bash
cd frontend
npm install
npm run dev
```

## Funcionalidades

- ✅ CRUD de pacientes
- ✅ CRUD de profissionais
- ✅ Agendamento de consultas com validações:
  - Paciente não pode ter 2 consultas no mesmo dia com o mesmo profissional
  - Profissional não pode atender 2 consultas simultâneas
  - Horário comercial: 08:00 às 18:00, segunda a sexta
  - Consultas de 30 minutos
- ✅ Autenticação JWT
- ✅ Documentação Swagger
- ✅ Logs em arquivo
- ✅ Testes unitários

## Usuário de teste
- **Email**: admin@clinic.com
- **Senha**: 123456
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
