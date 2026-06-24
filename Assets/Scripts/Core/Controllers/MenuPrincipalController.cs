using Assets.Scripts.Core.Enuns;
using UnityEngine;
using UnityEngine.UI; // Necess�rio para controlar o componente Button
using TMPro; // Necess�rio para controlar o texto
using UnityEngine.SceneManagement;
using SQLite4Unity3d;
using System.Linq;
using System.Collections.Generic;

public class MenuPrincipalController : MonoBehaviour
{
    [Header("Botões da Interface")]
    public Button btnNovoJogo;
    public Button btnContinuar;
    public Button btnSair;

    [Header("Botões de pesquisa")]
    public TMP_InputField Filtro;
    public TextMeshProUGUI TerminalUI;

    private void Start()
    {
        // Assim que o menu abre, validamos o que pode ou n�o ser clicado
        VerificarEstadoBotoes();
    }

    private void VerificarEstadoBotoes()
    {
        var db = DatabaseService.Instance.Connection;

        // Checa se existe algum registro na tabela Jogador
        int saveExistente = db.Table<Jogador>().Count();
        Debug.Log("Saves: " + saveExistente);

        if (saveExistente > 0)
        {
            // Save encontrado: libera o bot�o Continuar
            btnContinuar.interactable = true;
        }
        else
        {
            // Nenhum save: bloqueia o bot�o Continuar
            btnContinuar.interactable = false;
        }

        // Os bot�es Novo Jogo e Sair sempre ficam habilitados
        btnNovoJogo.interactable = true;
        btnSair.interactable = true;
    }

    //M�todo auxiliar para limpar o Save �nico
    private void ApagarSaveExistente()
    {
        var db = DatabaseService.Instance.Connection;

        // Limpa as tabelas din�micas atreladas � "run" e ao personagem
        // Isso preserva o cat�logo base de itens intacto
        db.DeleteAll<Jogador>();
        db.DeleteAll<Corpo>();
        db.DeleteAll<Membro>();
        db.DeleteAll<Origem>();
        db.DeleteAll<Inventario>();
        db.DeleteAll<ItemInstance>();
        db.DeleteAll<Inventario_Item>();
    }


    // Funçãoo que será chamada pelo botão "Novo Jogo" na Interface
    public void AoClicarCriarPersonagem()
    {
        var db = DatabaseService.Instance.Connection;

        // Se o botãoo for clicado mas já existir um save, apagamos ele primeiro
        if (db.Table<Jogador>().Count() > 0)
        {
            ApagarSaveExistente();
        }

        CriarSaveDoJogador();

        // Atualiza a interface logo após criar o personagem, liberando o botão "Continuar"
        VerificarEstadoBotoes();
    }

    // Função que será chamada pelo botão "Iniciar Raid"
    public void AoClicarIniciarJogo()
    {
        Debug.Log("Carregando o mapa...");
        // Carrega a cena de Gameplay (certifique-se de que o nome é o mesmo salvo no projeto)
        SceneManager.LoadScene("Gameplay_Scene");
    }

    private void CriarSaveDoJogador()
    {
        var db = DatabaseService.Instance.Connection;

        var itens = db.Table<Item>().ToList();

        db.BeginTransaction();
        try
        {
            // 1. Cria o Corpo 
            var novoCorpo = new Corpo { nivel = 1, xp = 0, energia = 100, energiaMax = 100, sanidade = 100, sanidadeMax = 100, fome = 100, sede = 100, sono = 0 };
            db.Insert(novoCorpo);
            Debug.Log(db.Table<Corpo>().FirstOrDefault());
            int corpoId = novoCorpo.ID;

            // 2. Cria os Membros
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.cabeca.GetHashCode(), saude = 80, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.pescoco.GetHashCode(), saude = 60, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.peito.GetHashCode(), saude = 100, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.barriga.GetHashCode(), saude = 120, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.pernaEsquerda.GetHashCode(), saude = 100, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.pernaDireita.GetHashCode(), saude = 100, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.canelaEsquerda.GetHashCode(), saude = 120, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.canelaDireita.GetHashCode(), saude = 120, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.bracoEsquerdo.GetHashCode(), saude = 100, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.bracoDireito.GetHashCode(), saude = 100, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.antebracoEsquerdo.GetHashCode(), saude = 120, quebrado = false, sangrando = false, infeccionado = false });
            db.Insert(new Membro { Corpo_ID = corpoId, nome = NomeMembro.antebracoDireito.GetHashCode(), saude = 120, quebrado = false, sangrando = false, infeccionado = false });

            // 3. Cria o Jogador
            var novoJogador = new Jogador { Corpo_ID = corpoId, nome = "Sobrevivente Anônimo" };
            db.Insert(novoJogador);

            // 4. Cria a Origem Permanente
            var origemJogador = new Origem { Dono_ID = novoJogador.ID, tipoOrigem = TipoOrigem.Jogador.GetHashCode(), permanente = true };
            db.Insert(origemJogador);

            // 5. Cria o Inventário atrelado à Origem
            var inventarioJogador = new Inventario { Origem_ID = origemJogador.ID, capacidade = 20, espaco = 0 };
            db.Insert(inventarioJogador);

            // 6. Instancia os Itens Iniciais (ItemInstance) e coloca o Item no Inventário do Jogador (Inventario_Item)
            var itemInicial = new ItemInstance { Item_ID = (itens.First(x => x.nome == "Glock 17").ID), espaco = 2, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f) };
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioJogador.Origem_ID, equipado = true, posX = null, posY = null });

            itemInicial = new ItemInstance { Item_ID = (itens.First(x => x.nome == "Standard 9x19mm").ID), espaco = 1, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f), stack = 15 };
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioJogador.Origem_ID, equipado = true, posX = null, posY = null });

            itemInicial = new ItemInstance { Item_ID = (itens.First(x => x.nome == "std Glock mag").ID), espaco = 1, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f) };
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioJogador.Origem_ID, equipado = true, posX = null, posY = null });


            itemInicial = new ItemInstance { Item_ID = (itens.First(x => x.nome == "Water Bottle").ID), espaco = 1, durabilidade = Random.Range(0.75f, 1f), qualidade = Random.Range(0.75f, 1f), stack = 2 };
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioJogador.Origem_ID, equipado = false, posX = null, posY = null });

            itemInicial = new ItemInstance { Item_ID = (itens.First(x => x.nome == "Canned food").ID), espaco = 1, durabilidade = Random.Range(0.75f, 1f), qualidade = Random.Range(0.75f, 1f), stack = 1};
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioJogador.Origem_ID, equipado = false, posX = null, posY = null });

            itemInicial = new ItemInstance { Item_ID = (itens.First(x => x.nome == "Dry meat").ID), espaco = 1, durabilidade = Random.Range(0.75f, 1f), qualidade = Random.Range(0.75f, 1f), stack = 3};
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioJogador.Origem_ID, equipado = false, posX = null, posY = null });

            db.Commit();
            Debug.Log("Save do jogador criado com sucesso!");
        }
        catch (System.Exception ex)
        {
            db.Rollback();
            Debug.LogError("Erro ao criar save do jogador: " + ex.Message);
        }
    }

    public void AoClicarBuscarInventario()
    {
        // Pega a string digitada no Input Field
        string textoFiltro = Filtro.text;

        // Limpa o texto da tela antes de realizar uma nova busca
        if (TerminalUI != null)
        {
            TerminalUI.text = $"=== INVENT�RIO (Filtro: '{textoFiltro}') ===\n";
        }

        // Chama a rotina SQL passando a string
        BisbilhotarInventario(textoFiltro);
    }

    public void BisbilhotarInventario(string filtroDeBusca = "")
    {
        var db = DatabaseService.Instance.Connection;

        string sql = @"
        SELECT 
            Item.nome AS NomeItem,
            Item.peso AS Peso,
            Item.valor AS Valor,
            ItemInstance.durabilidade AS Durabilidade,
            ItemInstance.qualidade AS Qualidade,
            ItemInstance.stack AS Quantidade
        FROM Jogador
        INNER JOIN Origem ON Jogador.ID = Origem.Dono_ID AND Origem.tipoOrigem = 3
        INNER JOIN Inventario_Item ON Origem.ID = Inventario_Item.Inventario_ID
        INNER JOIN ItemInstance ON Inventario_Item.Item_instance_ID = ItemInstance.ID
        INNER JOIN Item ON ItemInstance.Item_ID = Item.ID
        WHERE Item.nome LIKE ?
        ";

        // O parâmetro '?' evita SQL Injection e aplica o filtro
        string parametroBusca = $"%{filtroDeBusca}%";

        // Executa a query e converte o resultado para a nossa lista de DTOs
        List<ItemInventarioDTO> itensEncontrados = db.Query<ItemInventarioDTO>(sql, parametroBusca);

        TerminalUI.text = $"=== INVENTÁRIO DO JOGADOR (Filtro: '{filtroDeBusca}') ===\n\n";

        if (itensEncontrados.Count == 0)
        {
            //Debug.Log("Nenhum item encontrado.");
            TerminalUI.text += "Nenhum item encontrado.";
        }
        else
        {
            int? temp;
            foreach (var item in itensEncontrados)
            {
                if (item.Quantidade == null || item.Quantidade == 0)
                {
                    temp = 1;
                }
                else
                {
                    temp = item.Quantidade;
                }
                //Debug.Log($"- {item.NomeItem} | Peso: (X:{item.Peso}| Valor:{item.Valor} | Durabilidade: {item.Durabilidade}% | Qualidade: {item.Qualidade} )");
                TerminalUI.text += $"{item.NomeItem} ({temp}x) | Peso: {item.Peso*temp}g | Valor: R${item.Valor*temp} | Durabilidade: {(item.Durabilidade * 100):F2}% | Qualidade: {(item.Qualidade * 100):F2}% \n";
            }
        }

        TerminalUI.text +=  "=============================================";
    }

    //A��o do bot�o "Sair"
    public void AoClicarSair()
    {
        // Fecha o jogo na build final
        Application.Quit();

        // (Opcional) P�ra a execu��o se voc� estiver testando dentro do Editor da Unity
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
