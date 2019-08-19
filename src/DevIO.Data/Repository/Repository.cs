using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    // Este é o Repositório genérico;
    // abstract - ela só pode ser herdada (não pode ser instanciada);
    // o "new()", é para que possa dar um new da Entity (utilizado no método de remover por exemplo);
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        // "protected" porque tanto o Repository, quanto quem herder de Repository, vão poder ter acesso ao DbContext;
        protected readonly MeuDbContext Db;

        // um atalho para o DbSet - para não precisar usar assim: Db.Set<TEntity>().Add(entity) - substitui então por: DbSet.Add(entity);
        protected readonly DbSet<TEntity> DbSet;

        public Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            // o Tracking do Entity Framework: Toda vez que você coloca alguma coisa na memória, ele começa a fazer o Tracking, ele começa rastrear esse
            // objeto, para perceber mudanças de estado, etc; só que se você faz a leitura deste objeto, sem intenção de devolver ele para base, apenas por ler,
            // ele fica no tracking, ou seja, essa consulta ela gasta um pouco mais, ela acaba sendo um pouco mais honerosa para a aplicação; então se você quer
            // ter performance, use o "AsNoTracking", você vai retornar a resposta da leitura do banco, porém sem o tracking que vai te proporcionar um pouco
            // mais de performance;
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
            // isso significa: "Vá até o banco de dados, para aquela entidade específica, onde a expressão que você passar (predicate), retorna uma lista de forma assíncrona;

            // o await faz o seguinte: ele espera esse resultado acontecer, porque se você não esperar, esse resultado retorna uma Task,
            // e você não quer retornar uma Task, você quer retornar um resultado do banco, então você tem que dar um "await" para que ele converta esta Task no resultado que você espera;
            // o uso de await é sempre importante, porque senão você não tem certeza se você vai receber o resultado ou não;
        }

        // Não pode declarar o async na Interface, por isso tem que transformar os métodos em "async" nos métodos (adicionar o "async");

        // os métodos (com exceção do Buscar, que já é genérico o suficiente), foram transformados em virtual, porque além de tudo, vai poder dar um override nestes métodos;
        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            // Db.Set<TEntity>().Add(entity);
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            // para remover através de um id, precisa ir até o banco buscar o objeto, para depois ir no banco e devolver,
            // porque ele não remove pelo id, ele só remove recebendo a entidade em si;
            // DbSet.Remove(await DbSet.FindAsync(id));

            // como resolver sem ir ao banco buscar (ele só precisa do id de referência, não precisa do objeto todo):
            // var entity = new TEntity { Id = id };
            // DbSet.Remove();

            DbSet.Remove(new TEntity { Id = id });

            await SaveChanges();
        }

        // este não é virtual porque não quero precisar sobreescrever o SaveChanges, ele vai ficar focado apenas na classe de Repositorio genérica;
        // o resto se quiser modificar o comportamento dentro de uma classe de repositório específica, especializada para uma entidade x, irá poder;
        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            // o "?" é uma questão de boa prática, para que se ele existir, faça o Dispose, se ele não existir não faça - para evitar qualquer tipo de NullReferenceException;
            Db?.Dispose();
        }
    }
}
