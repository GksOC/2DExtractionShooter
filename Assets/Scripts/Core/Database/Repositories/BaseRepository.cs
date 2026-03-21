using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class BaseRepository<T> where T : new()
{
    // Mudamos para 'protected' para que repositórios filhos possam usar a conexão para Transactions
    protected SQLiteConnection db;

    public BaseRepository(SQLiteConnection connection)
    {
        db = connection;
        db.CreateTable<T>(); // Garante que a tabela exista ao iniciar
    }

    public List<T> GetAll() => db.Table<T>().ToList();

    public T GetById(int id) => db.Find<T>(id);

    public int Insert(T entity) => db.Insert(entity);

    public int Update(T entity) => db.Update(entity);

    public int Delete(T entity) => db.Delete(entity);
}