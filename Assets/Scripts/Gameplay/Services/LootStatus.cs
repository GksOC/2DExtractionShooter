using UnityEngine;
using Assets.Scripts.Core.Database.Services;

public class LootStatus : MonoBehaviour
{
    private LootService _service;
    private int ID;

    public void InicializarLoot(int ID, LootService service)
    {
        this.ID = ID;
        _service = service;
    }

    // A Unity chama isso automaticamente quando algo entra no Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        JogadorStatus jogador = collision.GetComponent<JogadorStatus>();

        if (jogador != null)
        {
            // 2. Aciona o Banco de Dados assincronamente (Loot -> Banco)
            _ = _service.ColetarItemAsync(ID, jogador.GetID());

            Debug.Log($"[Loot] O jogador pegou o item {ID}!");

            // 3. Remove o objeto do chão imediatamente
            Destroy(gameObject);
        }
    }
}