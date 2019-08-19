using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        // O Repository é uma classe abstrata, então não pode pode criar uma instancia dela, então se ele recebe alguma coisa no construtor,
        // alguém vai ter que passar pra ela; Então aqui, através do construtor desta classe, está recebendo o MeuDbContext e passar para a classe base esse contexto;
        public ProdutoRepository(MeuDbContext context) : base(context)  { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            // "Traga dados de produto, faça um inner join com Fornecedor, onde o produto tiver esse id informado";
            return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
                .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }
    }
}
