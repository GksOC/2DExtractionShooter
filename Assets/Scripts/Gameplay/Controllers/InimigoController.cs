using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Core.Enuns;

public class InimigoController : MonoBehaviour
{
    public float velocidadeMax;
    public float aceleracao;
    public float friccao;
    public Rigidbody2D rb;

    private Vector2 inputs;

    [Header("Sistema de Arma")]
    // Referência ao GameObject filho que representa a arma
    public GameObject armaVisual;

    private bool armaSacada = false;

    // Start is called before the first frame update
    void Start()
    {
        // Garante que o jogador inicie com a arma guardada
        if (armaVisual != null)
        {
            armaSacada = false;
            armaVisual.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x - rb.velocity.x / velocidadeMax * friccao * Time.deltaTime, 
                                  rb.velocity.y - rb.velocity.y / velocidadeMax * friccao * Time.deltaTime);
    }
}
