using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JogadorController : MonoBehaviour
{
    public float velocidadeMax;
    public float aceleracao;
    public float friccao;
    public Rigidbody2D rb;

    private Vector2 inputs;

    [Header("Sistema de Arma")]
    // Referência ao GameObject filho que representa a arma
    public GameObject armaVisual;

    private Camera mainCamera;
    private Vector2 mousePos;

    private bool armaSacada = false;

    // Start is called before the first frame update
    void Start()
    {
        //Configurar sempre a câmera como referência
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("[Jogador] Main Camera não encontrada na cena!");
        }

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
        //1  para trocar de arma
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AlternarSaqueDaArma();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("Boot_Scene");
        }

        //obter a posição do mouse em relação ao mundo
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //movimentar o personagem e rotação (mira)
        movimentar();
    }

    private void movimentar()
    {
        //captar os movimentos 
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //movimentar o personagem. Não esquecer do "normalized" para normalizar o vetor.
        // versão simples
        // rb.velocity = inputs.normalized * velocidade;

        // minha versão
        if(rb.velocity.magnitude < velocidadeMax)
        {
            if(inputs.x < 0 && rb.velocity.x > 0 || inputs.x > 0 && rb.velocity.x < 0)
            {
                //aceleração extra para mudar de direção
                //UnityEngine.Debug.Log("Mudando de direção");
                rb.velocity = new Vector2(rb.velocity.x + inputs.x * aceleracao * 4f * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                //aceleração normal
                rb.velocity = new Vector2(rb.velocity.x + inputs.x * aceleracao * Time.deltaTime, rb.velocity.y);
            }

            if(inputs.y < 0 && rb.velocity.y > 0 || inputs.y > 0 && rb.velocity.y < 0)
            {
                //aceleração extra para mudar de direção
                //UnityEngine.Debug.Log("Mudando de direção");
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + inputs.y * aceleracao * 4f * Time.deltaTime);
            }
            else
            {
                //aceleração normal
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + inputs.y * aceleracao * Time.deltaTime);
            }
        }
        

        rb.velocity = new Vector2(rb.velocity.x - rb.velocity.x / velocidadeMax * friccao * Time.deltaTime, 
                                  rb.velocity.y - rb.velocity.y / velocidadeMax * friccao * Time.deltaTime);
        if(inputs.x == 0) { 
            rb.velocity = new Vector2(rb.velocity.x - rb.velocity.x / velocidadeMax * friccao * 7 * Time.deltaTime, rb.velocity.y);
        }
        if (inputs.y == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - rb.velocity.y / velocidadeMax * friccao * 7 * Time.deltaTime);
        }

        //rotação do personagem para olhar na direção do mouse
        Vector2 direcaoOlhar = mousePos - rb.position;
        float angle = Mathf.Atan2(direcaoOlhar.y, direcaoOlhar.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

     private void AlternarSaqueDaArma()
    {
        if (armaVisual == null) { Debug.Log("Erro ao trocar de arma!"); return; };

        //Efeito de alternar o saque da arma
        armaSacada = !armaSacada;
        armaVisual.SetActive(armaSacada);
        Debug.Log("Arma está " + armaSacada);
    }
}
