using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Core.Database.Services;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Enuns;

public class ItemFactory : MonoBehaviour
{
    // Instância global para facilitar o acesso
    public static ItemFactory Instance { get; private set; }

    [Header("Configurações de Loot")]
    // Coloque aqui o Prefab do seu saquinho de Loot (que antes estava no InimigosFactory)
    public GameObject prefabLoot;

    private LootService _lootService;
    private SQLite4Unity3d.SQLiteConnection db;

    private void Awake()
    {
        // Configuração do Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // A fábrica instancia seus próprios serviços de comunicação com o banco
        db = DatabaseService.Instance.Connection;
        _lootService = new LootService(db);
    }

    public void GerarLootNoMundo(Inventario_Item item, Vector3 posicaoMorte)
    {
        posicaoMorte.x += Random.Range(-0.5f, 0.5f);
        posicaoMorte.y += Random.Range(-0.5f, 0.5f);
        GameObject lootNoChao = Instantiate(prefabLoot, posicaoMorte, Quaternion.identity);

        LootStatus lootStatus = lootNoChao.GetComponent<LootStatus>();
        //LootStatus.InicializarLoot(lootNoChao.GetInstanceID, );

        // TODO no futuro: lootNoChao.GetComponent<LootController>().InicializarItem(item);
        Debug.Log($"[Drop] Dropou um item com ID de Instância: {item.Item_instance_ID}");

        // Inicializa o script de Loot anexado a ele, passando o serviço para ele já saber como se coletar depois
        LootStatus status = lootNoChao.GetComponent<LootStatus>();
        if (status != null)
        {
            status.InicializarLoot(item.Item_instance_ID, _lootService);
        }
    }
}