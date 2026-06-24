using SQLite4Unity3d;
using System.Threading.Tasks;
using Assets.Scripts.Core.Models;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Scripts.Core.Database.Services
{
    public class InimigoService
    {
        private SQLiteConnection _db;
        private Origem mundo;

        public InimigoService(SQLiteConnection connection, Origem mundo)
        {
            _db = connection;
            this.mundo = mundo;
        }
        public CorpoCompletoDTO GetCorpo(int corpoID)
        {
            CorpoCompletoDTO ccDTO = new CorpoCompletoDTO();
            ccDTO.membros = _db.Table<Membro>().Where(x => x.Corpo_ID == corpoID).ToList();
            ccDTO.corpo = _db.Table<Corpo>().First(x => x.ID == corpoID);
            return ccDTO;
        }

        public async Task<List<Inventario_Item>> ObterItensAsync(int ID)
        {
            return await Task.Run(() => {
                Origem origemInimigo = _db.Table<Origem>().First(x => x.Dono_ID == ID);

                List<Inventario_Item> itensInventario = _db.Table<Inventario_Item>().Where(x => x.Inventario_ID == origemInimigo.ID).ToList();
                for (int i = 0; i< itensInventario.Count; i++)
                {
                    itensInventario[i].Inventario_ID = mundo.ID;
                    itensInventario[i].equipado = false;
                    itensInventario[i].posX = 0;
                    itensInventario[i].posY = 0;

                    //Como a chave primária de Iventario_Item é Iten_Instance_ID, o update da chave estrangeira Inventário_ID funcionará sem problemas
                    _db.Update(itensInventario[i]);
                }
                return itensInventario;
            });
        }

        public async Task DestruirInimigoAsync(int ID, int corpoID)
        {
            await Task.Run(() =>
            {
                Origem origemInimigo = _db.Table<Origem>().First(x => x.Dono_ID == ID);

                _db.BeginTransaction();
                try
                {
                    _db.Delete<Inventario>(origemInimigo.ID);
                    _db.Delete<Origem>(origemInimigo.ID);
                    _db.Delete<Inimigo>(ID);
                    _db.Delete<Membro>(corpoID);
                    _db.Delete<Corpo>(corpoID);
                }
                catch (System.Exception ex)
                {

                }
                _db.Commit();
            });
        }
    }
}