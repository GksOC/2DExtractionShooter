using SQLite4Unity3d;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.Database.Services
{
    public class JogadorService
    {
        private SQLiteConnection _db;

        public JogadorService(SQLiteConnection connection)
        {
            _db = connection;
        }

        // Adicionamos "Async" no nome e mudamos o retorno para Task
        public async Task<object> ExemploAsync()
        {
            // O Task.Run empurra a execução do banco para um núcleo secundário do processador
            await Task.Run(() =>
            {
                // Como não estamos acessando nada da Unity (Transform, GameObject), isso é 100% seguro
                
            });
            return null;
        }

        public CorpoCompletoDTO GetCorpo(int corpoID)
        {
            CorpoCompletoDTO ccDTO = new CorpoCompletoDTO();
            ccDTO.membros = _db.Table<Membro>().Where(x => x.Corpo_ID == corpoID).ToList();
            ccDTO.corpo = _db.Table<Corpo>().First(x => x.ID == corpoID);
            return ccDTO;
        }

        public async Task<CorpoCompletoDTO> CheckupAsync(int corpoID, CorpoCompletoDTO ccDTO)
        {
            bool check = false, check2 = false;
            var c = _db.Table<Corpo>().First(x => x.ID == corpoID);

            await Task.Run(() =>
            {
                foreach(Membro membro in ccDTO.membros)
                {
                    var m = _db.Table<Membro>().Where(x => x.Corpo_ID == corpoID).Where(x => x.ID == membro.ID).First();
                    if (m.sangrando != membro.sangrando) check = true;
                    if (m.quebrado != membro.quebrado) check = true;
                    if (m.infeccionado != membro.infeccionado) check = true;
                    if (m.saude != membro.saude) check = true;
                }
                if (ccDTO.corpo.nivel != c.nivel) check2 = true;
                if (ccDTO.corpo.xp != c.xp) check2 = true;
                if (ccDTO.corpo.energia != c.energia) check2 = true;
                if (ccDTO.corpo.energiaMax != c.energiaMax) check2 = true;
                if (ccDTO.corpo.sanidade != c.sanidade) check2 = true;
                if (ccDTO.corpo.sanidadeMax != c.sanidadeMax) check2 = true;
                if (ccDTO.corpo.fome != c.fome) check2 = true;
                if (ccDTO.corpo.sede != c.sede) check2 = true;
                if (ccDTO.corpo.sono != c.sono) check2 = true;
            });
            if (check) ccDTO.membros = _db.Table<Membro>().Where(x => x.Corpo_ID == corpoID).ToList();
            if (check2) ccDTO.corpo = c;
            if (check || check2) return ccDTO;
            return null;
        }
    }
}