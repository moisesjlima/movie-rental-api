# movie-rental-api
POC para testar os conhecimentos

### Objetivo:
* Implementar uma WebAPI REST
* A API do IMDB, Acessado via o site: http://www.omdbapi.com/, gerei a api_key para conectar na api conforme documentação em "Usage"
* Utilizei a versão 6.0 do.NET
* Utilizei o ORM [Entity Framework](https://learn.microsoft.com/pt-br/ef/) e banco de dados em memória
* Swagger está habilitado, utilizei para realizar os testes
* Utilizei o [Linq](https://learn.microsoft.com/pt-br/dotnet/csharp/programming-guide/concepts/linq/) para fazer as buscas e filtros

### APP:
Implementar uma api de uma locadora: para cada item criar uma rota:
- Deve ser possível cadastrar um cliente: fazer um POST com os dados do cliente
- Deve ser possível buscar um cliente por id: fazer um GET com o id do cliente
- Deve ser possível buscar uma lista de clientes procurando por nome ou inicial do nome: fazer um GET com o nome do cliente
- Deve ser possível atualizar os dados do cliente: fazer um PUT/PATCH para atualizar número de telefone
- Deve ser possível remover um cliente: fazer um DELETE para remover o cliente
- Deve ser possível buscar na api o filme por nome: fazer um GET para chamar a api do imdb procurando o filme por nome e retornando a lista de resultados
- Deve ser possível alugar um filme: fazer um POST para relação entre o imdbID do filme da lista retornada e id do cliente
- Deve ser possivel buscar na api todos os alugueis que existem: fazer um GET na tabela de alugueis
- Deve ser possível finalizar um aluguel de um filme: fazer um DELETE para remover a relação de cliente e filme

### RESTRIÇÕES/REGRAS DE NEGÓCIO:
* Menores de 14 anos, não podem alugar filmes, retornar HTTP Forbidden Error
* Não pode cadastrar 2 clientes com o mesmo email e CPF, retornar HTTP Conflict Error
* Não pode remover um cliente que tem um filme alugado, retornar HTTP Forbidden Error
* Um filme já alugado, não pode ser alugado por outra pessoa, enquanto o aluguel não tiver sido terminado
* Uma pessoa só pode alugar no máximo 2 filmes, enquanto não encerrar o tempo de aluguel do filme já alugado, não poderá pegar mais, retornar retornar HTTP Forbidden Error
