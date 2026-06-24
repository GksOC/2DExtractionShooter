using Assets.Scripts.Core.Enuns;
using Assets.Scripts.Core.Database.Services;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigosFactory : MonoBehaviour
{
    public GameObject prefabInimigo;
    public GameObject prefabLoot;
    public GameObject sangue;

    private InimigoService _service;
    private SQLite4Unity3d.SQLiteConnection db;

    // Start is called before the first frame update
    void Start()
    {
        db = DatabaseService.Instance.Connection;
        Origem mundo = db.Table<Origem>().First(x => x.tipoOrigem == TipoOrigem.Mundo.GetHashCode());
        _service = new InimigoService(db, mundo);

        GerarInimigosAleatorios();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GerarInimigosAleatorios()
    {
        // Exemplo: Gerando 10 inimigos
        for (int i = 0; i < 10; i++)
        {
            var novoInimigo = CriarInimigo();
            Vector3 pos;
            pos.x = Random.Range(-11.5f, 11.5f);
            pos.y = Random.Range(-6.5f, 6.5f);
            pos.z = 0f;

            GameObject inimigo = Instantiate(prefabInimigo, pos, Quaternion.identity);
            InimigoStatus status = inimigo.GetComponent<InimigoStatus>(); 

            // 2. Injeta o ID único do inimigo e a referência do serviço compartilhado
            status.InicializarInimigo(novoInimigo.ID, novoInimigo.Corpo_ID, _service, prefabLoot, sangue);
            Debug.Log("Inimigo ID: " + novoInimigo.ID + " criado!");
        }
    }

    private Inimigo CriarInimigo()
    {
        var itens = db.Table<Item>().ToList();
        Inimigo novoInimigo;

        // 1. Cria o Corpo 
        var novoCorpo = new Corpo { nivel = Random.Range(1, 10), xp = 0, energia = 100, energiaMax = 100, sanidade = 100, sanidadeMax = 100, fome = 100, sede = 100, sono = 0 };
        db.Insert(novoCorpo);
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

        // 3. Cria o Inimigo
        string tmp = UnityEngine.Random.Range(0, 65383).ToString();
        novoInimigo = new Inimigo { Corpo_ID = corpoId, nome = "Soldado #"+tmp, alcanceVisao = 100, anguloVisao = 90, tempoReacao = 1, precisao = 0.8f };
        db.Insert(novoInimigo);

        // 4. Cria a Origem Permanente
        var origemInimigo = new Origem { Dono_ID = novoInimigo.ID, tipoOrigem = TipoOrigem.Inimigo.GetHashCode(), permanente = false };
        db.Insert(origemInimigo);

        // 5. Cria o Inventário atrelado à Origem
        var inventarioInimigo = new Inventario { Origem_ID = origemInimigo.ID, capacidade = 20, espaco = 0 };
        db.Insert(inventarioInimigo);

        // 6. Cria Itens aleatórios para o Inventário
        //arma
        var armas = db.Table<Arma>().ToList();
        var armaInicial = new ItemInstance 
        { 
            Item_ID = armas[Random.Range(0, armas.Count)].Item_ID,
            espaco = 2, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f) 
        };
        db.Insert(armaInicial);
        db.Insert(new Inventario_Item { Item_instance_ID = armaInicial.ID, Inventario_ID = inventarioInimigo.Origem_ID, equipado = true, posX = null, posY = null });

        //carregador
        var carregadores = db.Table<Carregador>().ToList();
        var itemInicial = new ItemInstance
        {
            Item_ID = carregadores.FirstOrDefault(x => x.tipoMunicao == armas.FirstOrDefault(y => y.Item_ID == armaInicial.Item_ID).tipoMunicao).Item_ID,
            espaco = 1, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f), stack = Random.Range(7, 15)
        };
        db.Insert(itemInicial);
        db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioInimigo.Origem_ID, equipado = true, posX = null, posY = null });

        //munição
        var consumiveis = db.Table<Consumivel>().ToList();
        itemInicial = new ItemInstance 
        { 
            Item_ID = consumiveis.FirstOrDefault(x => x.tipoConsumivel == armas.FirstOrDefault(y => y.Item_ID == armaInicial.Item_ID).tipoMunicao ).Item_ID,
            espaco = 1, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f), stack = Random.Range(5, 25) 
        };
        db.Insert(itemInicial);
        db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioInimigo.Origem_ID, equipado = true, posX = null, posY = null });

        //bebida
        if(Random.Range(0,1) > 0) 
        {
            itemInicial = new ItemInstance
            {
                Item_ID = consumiveis.FirstOrDefault(x => x.tipoConsumivel == TipoConsumivel.bebida.GetHashCode()).Item_ID,
                espaco = 1, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f), stack = 1
            };
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioInimigo.Origem_ID, equipado = true, posX = null, posY = null });
        }

        //comida
        var comidas = consumiveis.Where(x => x.tipoConsumivel == TipoConsumivel.comida.GetHashCode()).ToList();
        for(int i = 0; i < Random.Range(0, 2); i++)
        {
            itemInicial = new ItemInstance
            {
                Item_ID = comidas[Random.Range(0, comidas.Count)].Item_ID,
                espaco = 1, durabilidade = Random.Range(0.65f, 0.85f), qualidade = Random.Range(0.65f, 0.85f), stack = 1
            };
            db.Insert(itemInicial);
            db.Insert(new Inventario_Item { Item_instance_ID = itemInicial.ID, Inventario_ID = inventarioInimigo.Origem_ID, equipado = true, posX = null, posY = null });
        }

        return novoInimigo;
    }
}

