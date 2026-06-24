using UnityEngine;
using Assets.Scripts.Core.Database.Services;
using System.Collections.Generic;
using Assets.Scripts.Core.Models;

public class InimigoStatus : MonoBehaviour, IDano
{
    private InimigoService _service;

    private int ID;
    private int CorpoID;
    private Corpo corpo;
    private List<Membro> membros;
    private int cabecaIndex;
    private int torsoIndex;

    private GameObject prefabLoot;
    private GameObject sangue;

    public int GetID()
    {
        return ID;
    }

    // Recebe o ID e o serviço da Factory
    public void InicializarInimigo(int ID, int CorpoID, InimigoService service, GameObject prefabLoot, GameObject sangue)
    {
        this.ID = ID;
        this.CorpoID = CorpoID;
        _service = service;
        this.prefabLoot = prefabLoot;
        this.sangue = sangue;
        CorpoCompletoDTO ccDTO = _service.GetCorpo(CorpoID);
        membros = ccDTO.membros;
        corpo = ccDTO.corpo;

        for( int i = 0; i < membros.Count; i++ )
        {
            //1 = cabeça, 3 = torso, olhar Enum NomeMembro.cs
            if (membros[i].nome == 1) cabecaIndex = i;
            if (membros[i].nome == 3) torsoIndex = i;
        }
    }

    public void ReceberDano(int dano)
    {
        //verificação de dano
        int i = Random.Range(0, membros.Count);
        while (membros[i].saude <= 0) { i = Random.Range(0, membros.Count); }
        membros[i].saude -= dano;

        //sangue
        Vector3 pos = new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + Random.Range(-0.3f, 0.3f), 0);
        Instantiate(sangue, pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) );

        //morte
        if (membros[cabecaIndex].saude <= 0 || membros[torsoIndex].saude <= 0) DestruirObjetoAsync();
    }

    private async void DestruirObjetoAsync()
    {
        //esconde o inimigo antes de apagar definitivamente e garantir as transações do banco
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        try
        { 
        List<Inventario_Item> itensDoInimigo = await _service.ObterItensAsync(ID);

        foreach (var item in itensDoInimigo)
        {
            // Instancia o objeto no mundo
            GameObject lootNoChao = Instantiate(prefabLoot, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), 
                                                transform.position.y + Random.Range(-0.5f, 0.5f), 0),
                                                Quaternion.identity);
            LootStatus lootStatus = lootNoChao.GetComponent<LootStatus>();
            //LootStatus.InicializarLoot(lootNoChao.GetInstanceID, );

            // TODO no futuro: lootNoChao.GetComponent<LootController>().InicializarItem(item);
            Debug.Log($"[Drop] Dropou um item com ID de Instância: {item.Item_instance_ID}");
        }

        await _service.DestruirInimigoAsync(ID, CorpoID);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Erro ao processar destruição do inimigo no banco: {ex.Message}");
        }

        Destroy(gameObject); 
    }
}