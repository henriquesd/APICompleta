using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    // Implementação de um conceito de repositório genérico;
    // Ele consiste em oferecer todos os métodos necessários para qualquer entidade;
    // Vai implementar a interface IDisposable, que é para obrigar que este repositório faça o dispose para liberar a memória;
    // Também vai ser específico, onde a TEntity só possa ser utilizada se ela for uma filha de Entity (where TEntity : Entity);
    // Neste repositório não poderia passar qualquer coisa, e se não especificar no where o que você está utilizando, 
    // você poderia passar qualquer coisa que seja um objeto; e para ser um repositório, persistir dados, tem que ser filha de Entity;
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity entity); // quando se tem uma Task apenas, é um método void;
        Task<TEntity> ObterPorId(Guid id); // retornar uma Task do tipo Entity;
        Task<List<TEntity>> ObterTodos(); // retorna uma lista de determinada entidade;
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        // um método buscar onde passa uma expression (uma expressão lambda), que vai trabalhar com uma function, que vai comparar
        // a sua entidade com alguma coisa desde que ela retorne um boolean; ou seja, está possibilidando que passe uma expressão
        // lambda dentro deste método para buscar qualquer entidade por qualquer parâmetro;
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        // ele vai retornar sempre um int, que é o número de linhas afetadas;
        Task<int> SaveChanges();
    }
}
