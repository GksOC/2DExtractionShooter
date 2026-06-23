using UnityEngine;
using SQLite4Unity3d;
using System.Linq;
using Assets.Scripts.Core.Enuns;
// Dependendo de como você organizou, adicione o using das suas Tabelas do Core:
// using Assets.Scripts.Core.Database.Tables; 

namespace Assets.Scripts.Gameplay.Generators
{
    public class JogadorGenerator : MonoBehaviour
    {
        [Header("Configurações de Instanciação")]
        public GameObject prefabJogador;

        public Transform pontoDeSpawn;

        private void Start()
        {
            GerarJogador();
        }

        private void GerarJogador()
        {
            // 1. Acessa a conexão do banco de dados estabelecida na Boot_Scene
            var db = DatabaseService.Instance.Connection; // O DatabaseService deve estar acessível globalmente

            // 2. Busca o registro do Jogador no banco
            var jogador = db.Table<Jogador>().FirstOrDefault();

            if (jogador == null)
            {
                Debug.LogError("[JogadorGenerator] Nenhum save de Jogador encontrado no banco!");
                return;
            }

            // 3. Instancia o Prefab visual/físico na Cena
            GameObject jogadorInstanciado = Instantiate(prefabJogador, pontoDeSpawn.position, Quaternion.identity);
            jogadorInstanciado.name = "Jogador_" + jogador.nome; // Renomeia na Hierarchy para ficar organizado

            Debug.Log($"[JogadorGenerator] {jogador.nome} instanciado com sucesso!");
        }
    }
}