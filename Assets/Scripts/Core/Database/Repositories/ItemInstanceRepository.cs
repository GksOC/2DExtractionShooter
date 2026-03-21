using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class ItemInstanceRepository : BaseRepository<ItemInstance>
{
    public ItemInstanceRepository(SQLiteConnection connection) : base(connection)
    {
    }

    // Aqui você adiciona regras de negócio ESPECÍFICAS dessa tabela
    public void MoverParaEsconderijo(ItemInstance item, int novoInventarioId)
    {
        // Exemplo de uso de Transaction exigido pelo seu GDD
        db.BeginTransaction();
        try
        {
            // Lógica para atualizar a Origem e o Inventario_Item
            // ...
            db.Update(item);
            db.Commit(); // Salva de forma segura
        }
        catch
        {
            db.Rollback(); // Se der erro (ex: falta de espaço), desfaz tudo para não perder o item
        }
    }
}