using UnityEngine;
using Assets.Scripts.Core.Enuns;
using System.Linq;
using System.Collections.Generic;
// Dependendo de como você organizou, adicione o using das suas Tabelas do Core:
// using Assets.Scripts.Core.Database.Tables; 

public class JogadorGenerator : MonoBehaviour
{
    [Header("Configurações de Instanciação")]
    public GameObject prefabJogador;
    public GameObject sangue;
    public Transform pontoDeSpawn;

    private void Start()
    {
        GerarJogador();
    }

    private void GerarJogador()
    {
        // 1. Acessa a conexão do banco de dados estabelecida na Boot_Scene
        var db = DatabaseService.Instance.Connection;

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

        //4. Passando a referência do BD para o objeto em runtime.
        var statusRuntime = jogadorInstanciado.GetComponent<JogadorStatus>();
        statusRuntime.InicializarJogador(jogador.ID, jogador.Corpo_ID, sangue);

        Debug.Log($"[JogadorGenerator] {jogador.nome} instanciado com sucesso!");


        //GERADOR DE ORIGEM TEMPORÁRIO, SUBSTITUIR POR UM SCRIPT PRÓPRIO DEPOIS
        Origem origemMundo = db.Table<Origem>().FirstOrDefault(x => x.tipoOrigem == TipoOrigem.Mundo.GetHashCode());
        if(origemMundo == null)
        {
            origemMundo = new Origem { Dono_ID = null, tipoOrigem = TipoOrigem.Mundo.GetHashCode(), permanente = false };
            db.Insert(origemMundo);
            var inventarioMundo = new Inventario { Origem_ID = origemMundo.ID, capacidade = 9999, espaco = 0 };
            db.Insert(inventarioMundo);
        }
    }
}