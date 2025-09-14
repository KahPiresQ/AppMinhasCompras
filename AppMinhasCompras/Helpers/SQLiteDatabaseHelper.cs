//adicionar using que vamos usar.

using AppMinhasCompras.Models;
using SQLite;


namespace AppMinhasCompras.Helpers
{
    //Alterar a classe de internal para public, para evitar falhas de acesso por reflexão (DI/XAML).
    public class SQLiteDatabaseHelper

    {
        readonly SQLiteAsyncConnection _conn;

        //construtor sempre é chamado quando o objeto é instânciado, onde esta o arquivo SQLite.Usei no nome cam ao inves de path, para caminho.
        public SQLiteDatabaseHelper(string cam)
        {
            _conn = new SQLiteAsyncConnection(cam);

            // Garante que a tabela exista. Se não existir, cria.
            _conn.CreateTableAsync<Produto>().Wait();
        }

        // INSERT adiciona um Produto novo no banco. Retorna 1 quando deu bom.
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // UPDATE atualiza os campos do Produto pelo Id.
        // aqui usamos UPDATE direto na string. Mantive igual à agenda.
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>( sql, p.Descricao, p.Quantidade, p.Preco, p.Id );
        }

        // DELETE apaga o Produto com esse Id. Se achou, apaga; se não, 0 linhas.
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // GET ALL traz a listinha completa dos Produtos (sem filtro).
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // SEARCH procura por descrição contendo "q".
        // CORREÇÃO  faltava o FROM 

        // SEARCH: procura por descrição OU categoria contendo "q" ag6
        public Task<List<Produto>> Search(string q)
        {
            string like = $"%{q}%";
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE ? OR Categoria LIKE ?";
            return _conn.QueryAsync<Produto>(sql, like, like);
        }
        // RELATÓRIO: soma de gastos por categoria fihcario 6
        public Task<List<CategoriaTotal>> GetTotaisPorCategoria()
        {
            string sql = @"
        SELECT 
            CASE 
                WHEN Categoria IS NULL OR Categoria = '' 
                THEN 'Sem categoria' 
                ELSE Categoria 
            END AS Categoria,
            SUM(Quantidade * Preco) AS Soma
        FROM Produto
        GROUP BY Categoria
        ORDER BY Soma DESC";

            return _conn.QueryAsync<CategoriaTotal>(sql);
        }
    }

}


