using Assets.Scripts.Core.Enuns;
using SQLite4Unity3d;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigosFactory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CriarInimigo(Vector2 posicao)
    {
        var db = DatabaseService.Instance.Connection;

        var itens = db.Table<Item>().ToList();

        db.BeginTransaction();
        try
        {
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
            var novoInimigo = new Inimigo { Corpo_ID = corpoId, nome = "Soldado #"+tmp, alcanceVisao = 10, anguloVisao = 90, tempoReacao = 1, precisao = 0.8f };
            db.Insert(novoInimigo);

            // 4. Cria a Origem Permanente
            var origemInimigo = new Origem { Dono_ID = novoInimigo.ID, tipoOrigem = TipoOrigem.Inimigo.GetHashCode(), permanente = true };
            db.Insert(origemInimigo);

            // 5. Cria o Inventário atrelado à Origem
            var inventarioInimigo = new Inventario { Origem_ID = origemInimigo.ID, capacidade = 20, espaco = 20 };
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

        }
        catch (System.Exception ex)
        {
            db.Rollback();
            Debug.LogError("Erro ao criar inimigo: " + ex.Message);
        }
    }
}

