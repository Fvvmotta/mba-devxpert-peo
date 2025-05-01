# **MBA DevXpert EduPlatform - Plataforma de Ensino Modular com DDD, CQRS e API Segura**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **MBA DevXpert EduPlatform**. Este projeto faz parte da formação **MBA DevXpert Full Stack .NET** e representa uma aplicação educacional com foco em arquitetura limpa (Clean Architecture) e design orientado a domínios (DDD).

O objetivo é prover uma plataforma de ensino modular que gerencia alunos, cursos, matrículas, certificados e pagamentos, utilizando ASP.NET Core, EF Core, MediatR e autenticação baseada em JWT.

### **Autor**
- Fernando Vinícius Valim Motta

---

## **2. Proposta do Projeto**

O objetivo deste projeto é desenvolver uma aplicação web moderna, modular e escalável voltada à gestão de conteúdo educacional, utilizando os princípios de arquitetura limpa e DDD. A aplicação possui:

- **Separação em Bounded Contexts**: Domínios isolados como Alunos, Conteúdo e Pagamentos.
- **API RESTful**: Recursos expostos para integração com sistemas externos.
- **Controle de Acesso**: Autenticação via JWT com perfis (Admin/Aluno).
- **Infraestrutura de Dados**: Banco SQL Server com uso de EF Core e Seed automático.
- **Experiência Dev**: Projeto modular, testável e documentado com Swagger.

---

## **3. Tecnologias Utilizadas**

- **Linguagem:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - Entity Framework Core
  - MediatR (CQRS)
- **Autenticação:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token)
- **Banco de Dados:** SQL Server (produção) / SQLite (desenvolvimento)
- **Documentação da API:** Swagger

---

## **4. Estrutura do Projeto**

```
src/
│
├── MBA_DevXpert_PEO.Api/                          # API principal (Swagger, Auth, DI)
├── MBA_DevXpert_PEO.Core/                         # Entidades base, VO, UoW, DTOs, Interfaces comuns
│
├── MBA_DevXpert_PEO.GestaoDeAlunos.*              # Domínio e aplicação de Alunos (matrículas, certificados)
├── MBA_DevXpert_PEO.GestaoDeConteudo.*            # Domínio e aplicação de Conteúdo (cursos, aulas)
├── MBA_DevXpert_PEO.PagamentoEFaturamento.*       # Domínio e aplicação de Pagamentos
├── MBA_DevXpert_PEO.Identidade/                   # Autenticação e configuração do Identity com JWT
├── EventSourcing/                                  # Suporte a histórico de eventos (opcional)
│
├── Tests/                                         # Testes unitários e de integração
│
├── README.md                                      # Documentação do projeto
├── FEEDBACK.md                                    # Feedback do avaliador
├── .gitignore                                     # Arquivos ignorados pelo Git
```

---

## **5. Funcionalidades Implementadas**

- **Gestão de Alunos:** Cadastro, edição, visualização e remoção de alunos, além de matrícula e conclusão de curso.
- **Gestão de Conteúdo:** CRUD completo de cursos e aulas.
- **Pagamentos e Faturamento:** Pagamento vinculado a matrícula, com status simulado de aprovação.
- **Certificados:** Geração automática após finalização do curso.
- **Autenticação:** Login por JWT com perfis de acesso (Admin / Aluno).
- **API RESTful:** Endpoints organizados por domínio com CQRS (MediatR).
- **Swagger:** Documentação interativa da API.
- **Seeder:** Base inicial com curso, aluno, matrícula e pagamento.

---

## **6. Como Executar o Projeto**

### **Pré-requisitos**
- .NET SDK 8.0 ou superior
- SQL Server ou SQLite
- Visual Studio 2022+ ou VS Code
- Git

### **Passos para Execução**

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/Fvvmotta/mba-devxpert-peo.git
   cd mba-devxpert-peo
   ```

2. **Executar o projeto:**
   - Configure sua `appsettings.json` com a connection string correta.
   - Execute o projeto via Visual Studio ou terminal com:
     ```bash
     dotnet run --project src/MBA_DevXpert_PEO.Api
     ```

3. **Acesso e autenticação:**
   - Admin: `admin@teste.com` / `Admin@123`
   - Aluno: `aluno@teste.com` / `Aluno@123`

4. **Swagger:**
   - Acesse a documentação em `http://localhost:<porta>/swagger`

---

## **7. Instruções de Configuração**

- **JWT:** Configurado no `appsettings.json` sob `JwtSettings`.
- **Migrações:** Realizadas automaticamente via `DbSeederExtension` ao iniciar a aplicação.
- **Perfis:** Configure roles e usuários na camada `Identidade`.

---

## **8. Documentação da API**

A API está documentada com Swagger. Acesse após subir o projeto:
```
http://localhost:<porta>/swagger
```
Use o botão **Authorize** para testar endpoints protegidos com Bearer Token.

---

## **9. Avaliação**

- Projeto acadêmico submetido para avaliação no MBA DevXpert.
- O arquivo `FEEDBACK.md` deve ser preenchido exclusivamente pelo instrutor.
- Dúvidas ou sugestões? Abra uma **Issue** no repositório.
