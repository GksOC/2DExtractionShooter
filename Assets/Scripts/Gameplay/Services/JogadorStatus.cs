using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Database.Services;
using Assets.Scripts.Core.Models;
using UnityEngine;


public class JogadorStatus : MonoBehaviour, IDano
{
    private JogadorService _service;
    private GameObject sangue;

    [Header("Identificação no Banco")]
    private int meuJogadorID;
    private int meuCorpoID;

    [Header("Status em Runtime (RAM)")]
    private List<Membro> membros;
    private Corpo corpo;
    private int cabecaIndex;
    private int torsoIndex;
    // ... outras variáveis de status do Corpo e Membros

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
            
    }

    // Esta é a função vital. O Generator a chamará logo após o Instantiate()
    public void InicializarJogador(int jogadorID, int corpoID, GameObject sangue)
    {
        this.meuJogadorID = jogadorID;
        this.meuCorpoID = corpoID;
        this.sangue = sangue;

        //ativando o serviço
        var conexao = DatabaseService.Instance.Connection;
        _service = new JogadorService(conexao);

        //trazendo os dados
        CorpoCompletoDTO ccDTO = _service.GetCorpo(corpoID);
        membros = ccDTO.membros;
        corpo = ccDTO.corpo;

        for (int i = 0; i < membros.Count; i++)
        {
            //1 = cabeça, 3 = torso, olhar Enum NomeMembro.cs
            if (membros[i].nome == 1) cabecaIndex = i;
            if (membros[i].nome == 3) torsoIndex = i;
        }


        // Assim que o serviço é ligado, disparamos o temporizador contínuo
        StartCoroutine(RotinaCheckUp());

        Debug.Log($"[Status] Fui inicializado! Meu ID no banco é {meuJogadorID} e meu Corpo é {meuCorpoID}");
    }

    public int GetID()
    {
        return meuJogadorID;
    }

    public void ReceberDano(int dano)
    {
        int i = Random.Range(0, membros.Count);
        while (membros[i].saude <= 0) { i = Random.Range(0, membros.Count); }
        membros[i].saude -= dano;

        Vector3 pos = new Vector3(transform.position.x + Random.Range(-0.15f, 0.15f), transform.position.y + Random.Range(-0.15f, 0.15f), 0);
        Instantiate(sangue, pos, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if (membros[cabecaIndex].saude <= 0 || membros[torsoIndex].saude <= 0) { Debug.Log("Você se matou xOx"); Application.Quit(); }
    }

    private IEnumerator RotinaCheckUp()
    {
        // O while(true) faz o loop rodar para sempre (enquanto o jogador existir na cena)
        while (true)
        {
            // Esta função pausa por 10 segundos virtuais,
            // devolvendo o processamento para o jogo principal neste meio tempo.
            yield return new WaitForSeconds(10f);

            CorpoCompletoDTO ccDTO = new CorpoCompletoDTO();
            ccDTO.membros = membros;
            ccDTO.corpo = corpo;
            var check = _service.CheckupAsync(meuCorpoID, ccDTO);
            if(check != null)
            {
                Debug.Log("Interferência no jogador detectada!");
            }

            // (Opcional) Remova o Debug depois para não poluir o console
            Debug.Log("[Status] Check-up no banco realizado (10s).");
        }
    }

}