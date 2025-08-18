# FEEDBACK – Revisão Técnica (Plataforma de Educação Online)

## Organização do Projeto

Pontos positivos:
- Estrutura limpa e modular por bounded contexts (`src/*` com `Alunos`, `Conteudos`, `Pagamentos`, `Api`, `Core`).
- Solution file `MBA_DevXpert_PEO.sln` presente na raiz e projetos de teste separados em `test/`.

Pontos negativos / observações:
- Existem warnings numerosos (nullable, possíveis NREs) que devem ser tratados para reduzir ruído e evitar bugs em produção (ex.: `src/**/Api` e `Pagamentos.Business`).
- Não foram encontradas pastas `Migrations/` versionadas no repositório para os contextos (busca por `Migrations` retornou vazio). Apesar disso, a aplicação chama `Database.MigrateAsync()` no seeder — isso funciona se as migrations existirem nas assemblies, mas dificulta análise offline.

---

## Modelagem de Domínio (DDD)

Pontos positivos:
- Os três bounded contexts estão presentes e com pastas distintas: `Conteudos`, `Alunos` e `Pagamentos`.
- Entidades e VOs esperados (ex.: `Curso`, `Aula`, `Aluno`, `Matricula`, `Pagamento`) existem e os agregados implementam regras (ex.: métodos de domínio nas entidades).

Pontos negativos:
- O projeto `Core` centraliza mensagens e alguns contratos.

---

## Casos de Uso e Regras de Negócio

Pontos positivos:
- Handlers, Commands e Queries implementados para operações básicas (criar curso, adicionar aula, matricular aluno).

Pontos negativos:
- Fluxos críticos testados apenas parcialmente; integrações end-to-end falham por problema de migração/seed, impedindo validação completa dos cenários (matrícula + pagamento + certificado).

---

## Infra / Migrations / Seed

O que foi verificado:
- `Program.cs` chama `await app.UseDatabaseSeeder();` quando o ambiente é `Development` ou `Testing`.
- `DbSeederExtension` executa `Database.MigrateAsync()` para os quatro contexts:
  - `GestaoConteudoContext`
  - `AlunosContext`
  - `PagamentosContext`
  - `IdentityContext`
- Em seguida o seeder popula cursos, usuário admin/aluno e uma matrícula de teste.

Problema encontrado (causa dos testes de integração falharem):
- Durante execução dos testes de integração, a chamada ao seeder tenta consultar `context.Cursos.Any()` antes das tabelas existirem — resultando em `SQLite Error 1: 'no such table: Cursos'`.
- Observação: a solução registra DbContexts com SQLite quando o ambiente é `Development` ou `Testing` (arquivo `DbSelectorExtension.cs`) e a `appsettings.Testing.json` define `DefaultConnection` apontando para `Data Source=devxpert_tests.db`.

Sugestões:
- Garantir que as migrations estejam incluídas nas assemblies (ou que a estratégia de criação seja compatível com o banco em tempo de teste). Ex.: criar migrations per-project e mantê-las versionadas ou usar EnsureCreated em testes, ou executar explicitamente `context.Database.Migrate()` antes de qualquer consulta no seeder.
- Tornar o seeder mais resiliente: envolver o fluxo de verificação em try/catch e executar `context.Database.EnsureCreated()` quando apropriado no ambiente de testes.

---

## Autenticação e Identidade (JWT)

Pontos positivos:
- Identity está configurado e JWT configurado via `AddIdentityConfig` em `IdentityConfig.cs`.
- `appsettings.json` contém `JwtSettings` com `Secret`, `Issuer`, `Audience`.

Pontos negativos / observações:
- Em `AppSettings.cs` nomes de propriedades não coincidem exatamente com keys em `appsettings.json` (`JwtSettings` contém `ExpirationHours`, `Issuer`, `Audience` — o `AppSettings` usa `ExpirationHours`/`Issuer`/`Audience` em partes, mas há campos extras como `RootFilePath` que não aparecem no JSON). Conferir o binding e nomes.

---

## Testes e Cobertura

Resultados observados (execução local):
- `dotnet test --collect:"XPlat Code Coverage"` retornou: total 70 testes, falharam 6 (integração). Build finalizou com erro devido aos 6 testes falhando; logs de teste e cobertura foram gerados.

Observação de conformidade:
- A cobertura global está muito abaixo do requisito de 80% (48.13%).

Recomendações:
- Aumentar testes unitários nos projetos de Application e Controllers; adicionar testes de integração que inicializem banco via EnsureCreated ou aplicar migrations antes do seeder.

---

## Documentação

Pontos positivos:
- `README.md` presente e descreve stack e execução básica.

Pontos negativos:
- Documentação operacional incompleta sobre como reproduzir o ambiente de testes (por exemplo: instruções explícitas para aplicar migrations, ou passo a passo para rodar testes de integração localmente).

Sugestão:
- Adicionar um parágrafo no README com comandos mínimos para preparar e rodar os testes (ex.: dotnet ef database update, dotnet test --collect...).

---

## Conclusão e próximos passos (ação recomendada)

Resumo:
- O repositório tem boa arquitetura inicial e separação por bounded contexts; contudo, falhas nos testes de integração (migrations/seed) e cobertura baixa reduzem a nota técnica atual.

Prioridade imediata (curto prazo):
1. Corrigir o fluxo de criação/migração do banco em ambiente de teste (garantir que `Cursos` exista antes do seeder consultar). Verificar `Migrations` ou usar `EnsureCreated()` no ambiente `Testing`.
2. Ajustar binding de `AppSettings`/JWT e tratar warnings nullable críticos.
3. Aumentar cobertura com testes unitários nas camadas de Application/Controllers e reforçar testes de integração com banco criado apropriado.

Sugestões de melhorias (médio prazo):
- Revisar acoplamentos entre bounded contexts e reduzir vazamento de contratos para o `Core`.
- Tratar warnings de nullable e melhorar análise estática (dotnet format/nullable).

---

## Matriz de Avaliação (NOTAS: inteiro 5..10) — pedir revisão antes de publicar final

| **Critério**                   | **Peso** | **Descrição** | **Nota** |
|--------------------------------|----------|-----------------|--------|
| **Funcionalidade**             | 30%      | Se atende aos requisitos funcionais. | 7 |
| **Qualidade do Código**        | 20%      | Clareza, organização, padrões. | 7 |
| **Eficiência e Desempenho**    | 20%      | Eficiência das soluções. | 7 |
| **Inovação e Diferenciais**    | 10%      | Criatividade e aspectos únicos. | 7 |
| **Documentação e Organização** | 10%      | Qualidade e completude da documentação. | 7 |
| **Resolução de Feedbacks**     | 10%      | Capacidade de responder feedbacks. | 7 |

🎯 Nota Final: 7.0 / 10
