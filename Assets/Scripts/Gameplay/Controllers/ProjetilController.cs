using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetilController : MonoBehaviour
{
    public float velocidade;
    // Filtro para não acertar o próprio jogador
    public LayerMask layerAlvo; 
    
    private Vector2 posicaoAnterior;
    private float tempoVida = 3f;

    void Start()
    {
        posicaoAnterior = transform.position;
    }

    void Update()
    {
        // Calcula o quanto o tiro vai andar neste frame exato
        float distanciaDoFrame = velocidade * Time.deltaTime;

        //Espelha o sprite
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x); // garante que o sinal seja correto
        transform.localScale = scale;
        
        // Dispara a linha invisível (Raycast) para ver se algo está no meio do caminho
        RaycastHit2D hit = Physics2D.Raycast(posicaoAnterior, transform.up, distanciaDoFrame, layerAlvo);

        if (hit.collider != null)
        {
            Debug.Log($"Acertou: {hit.collider.name}");
            
            //IMPLEMENTAR O SISTEMA DE ACERTO
            //TAMBÉM VERIFICAR PENETRAÇÃO DE PROJÉTIL
            // Destrói o tiro
            Destroy(gameObject); 
            return;
        }

        // Se não bateu em nada, move o tiro visualmente para frente
        transform.Translate(Vector2.up * distanciaDoFrame);
        
        // Atualiza a posição anterior para o cálculo do próximo frame
        posicaoAnterior = transform.position;

        // Diminui o tempo de vida do projétil
        tempoVida -= Time.deltaTime;
        if(tempoVida <= 0f)
        {
            Destroy(gameObject);
        }
    }
}