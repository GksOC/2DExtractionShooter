using SQLite4Unity3d;
using System.Threading.Tasks;
using Assets.Scripts.Core.Models;
using System.Collections.Generic;
using System.Linq;

public class LootService
{

    private SQLiteConnection _db;
    private Origem mundo;

    public LootService(SQLiteConnection connection)
    {
        _db = connection;
    }

    public async Task ColetarItemAsync(int item_ID, int jogador_ID)
    {
        await Task.Run(() =>
        {
            Origem jogador_origem = _db.Table<Origem>().First(x => x.Dono_ID == jogador_ID);
            int jogador_inv = _db.Table<Inventario>().First(x => x.Origem_ID == jogador_origem.ID).Origem_ID;

            string sql = @"
            UPDATE Inventario_Item SET
            Inventario_ID = ?
            WHERE Item_instance_ID = ?
            ";

            _db.Execute(sql, jogador_inv, item_ID);
        });
    }
}

