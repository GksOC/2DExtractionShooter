using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using Assets.Scripts.Core.Enuns;
using System;

namespace Assets.Scripts.Core
{
    public class StartGame : MonoBehaviour
    {
        private string dbPath;

        // Use this for initialization
        void Start()
        {
            // O caminho do banco de dados, o mesmo definido no DatabaseService
            dbPath = Path.Combine(Application.persistentDataPath, "ExtractionShooter.db");

            Debug.Log("[Servidor] Iniciando verificação do sistema de Banco de Dados...");

            //isso serve para apagar o bando de dados
            //DatabaseService.Instance.ResetarBancoDeDados();

            // Verifica fisicamente se o arquivo do banco já existia antes de abrir o jogo
            if (!File.Exists(dbPath))
            {
                Debug.Log("[Servidor] Banco de dados não encontrado. Gerando novo arquivo SQLite...");
            }
            else
            {
                Debug.Log("[Servidor] Banco de dados existente encontrado. Verificando integridade das tabelas...");
            }
            GerarTabelasBase();
            VerificarEPreencherItens();
            // LogTabelaItens();

            Debug.Log("Abrindo interface: ");
            SceneManager.LoadScene("MenuPrincipal_Scene");
        }

        private void GerarTabelasBase()
        {
            // Pega a conexão contínua do nosso Singleton
            var db = DatabaseService.Instance.Connection;

            try
            {
                // 1. Gerando as tabelas do Catálogo Base (Itens estáticos)
                db.CreateTable<Item>(); // [2]
                db.CreateTable<Arma>(); // [3]
                db.CreateTable<Armadura>(); // [4]
                db.CreateTable<Consumivel>(); // [5]
                db.CreateTable<Carregador>(); // [6]
                db.CreateTable<Anexo>(); // [7]
                db.CreateTable<Mochila>(); // [8]
                db.CreateTable<Explosivo>(); // [9]

                // 2. Gerando as tabelas Dinâmicas (Inventário e Instâncias)
                db.CreateTable<ItemInstance>(); // [10]
                db.CreateTable<Origem>(); // [11]
                db.CreateTable<Inventario>(); // [12]
                db.CreateTable<Inventario_Item>(); // [13]

                // 3. Gerando as tabelas de Entidades Vivas
                db.CreateTable<Corpo>(); // [14]
                db.CreateTable<Membro>(); // [15]
                db.CreateTable<Jogador>(); // [16]
                db.CreateTable<Inimigo>(); // [17]

                Debug.Log("[Servidor] Sucesso! Todas as tabelas base foram criadas/validadas no banco de dados.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Servidor] Erro crítico ao gerar as tabelas: {ex.Message}");
            }
        }

        // Verificar e preencher a tabela base
        private void VerificarEPreencherItens()
        {
            // Acessa a conexão central do Singleton
            var db = DatabaseService.Instance.Connection;

            // Verifica quantos registros existem na tabela Item
            int quantidade = db.Table<Item>().Count();

            // Se estiver vazia (0), criamos os itens iniciais
            if (quantidade == 0)
            {
                Debug.Log("[Servidor] Tabela 'Item' está vazia. Gerando itens base...");

                // Criando todas as instâncias dos modelos
                Item item;
                Arma arma;
                Consumivel consumivel;
                Anexo anexo;
                Carregador carregador;
                Armadura armadura;
                Mochila mochila;
                Explosivo explosivo;
                var configJSON = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };

                //Armas
                item = new Item
                { 
                    // O ID possui a constraint AutoIncrement, então omitidos para o SQLite gerar sozinho
                    tipoItem = TipoItem.Arma.GetHashCode(), 
                    nome = "AKM", 
                    peso = 3200, 
                    valor = 2700, 
                    imagem = Path.Combine("SVG", "AKM.svg")
                };
                db.Insert(item);

                arma = new Arma
                {
                    Item_ID = item.ID,
                    tipoAnexo = TipoAnexo.AK100.GetHashCode(),
                    tipoMunicao = TipoConsumivel._762x39.GetHashCode(),
                    compatibilidade = Compatibilidade.bocal.GetHashCode() + Compatibilidade.mira.GetHashCode(),
                    cadencia = 600,
                    velocidade = 670,
                    ergonomia = 65,
                    precisao = 1.0f
                };
                db.Insert(arma);


                item = new Item
                {
                    tipoItem = TipoItem.Arma.GetHashCode(),
                    nome = "Glock 17",
                    peso = 630,
                    valor = 1500,
                    imagem = Path.Combine("SVG", "Glock.svg")
                };
                db.Insert(item);

                arma = new Arma
                {
                    Item_ID = item.ID,
                    tipoAnexo = TipoAnexo.Handgun.GetHashCode(),
                    tipoMunicao = TipoConsumivel._9mm.GetHashCode(),
                    compatibilidade = Compatibilidade.bocal.GetHashCode() + Compatibilidade.mira.GetHashCode(),
                    cadencia = 300,
                    velocidade = 350,
                    ergonomia = 80,
                    precisao = 2.0f
                };
                db.Insert(arma);

                //Consumivel
                    //Bebida
                item = new Item 
                { 
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Water Bottle", 
                    peso = 500, 
                    valor = 10, 
                    imagem = Path.Combine("SVG", "water-bottle.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel.bebida.GetHashCode(),
                    capacidadeMax = 4,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { sede = 25, sanidadeInstantanea = 5, bonusRegeneracaoEnergia = 0.1f, tempoBonusRegeneracaoEnergia = 60 }, configJSON)
                };
                db.Insert(consumivel);

                    //Comida
                item = new Item 
                { 
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Canned food", 
                    peso = 500, 
                    valor = 20, 
                    imagem = Path.Combine("SVG", "canned-food.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel.comida.GetHashCode(),
                    capacidadeMax = 4,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { sede = 8, fome = 25, sanidadeInstantanea = 5, bonusRegeneracaoEnergia = 0.1f, tempoBonusRegeneracaoEnergia = 60, boostVelocidade = -0.1f, tempoBoostVelocidade = 20}, configJSON)
                };
                db.Insert(consumivel);


                item = new Item 
                { 
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Dry meat", 
                    peso = 100, 
                    valor = 10, 
                    imagem = Path.Combine("SVG", "dry-meat.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel.comida.GetHashCode(),
                    capacidadeMax = 10,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { sede = -10, fome = 20, sanidadeInstantanea = 2, bonusRegeneracaoEnergia = 0.12f, tempoBonusRegeneracaoEnergia = 60, boostVelocidade = 0.05f, tempoBoostVelocidade = 20}, configJSON)
                };
                db.Insert(consumivel);

                item = new Item
                {
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "MRE",
                    peso = 2500,
                    valor = 500,
                    imagem = Path.Combine("SVG", "MRE.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel.comida.GetHashCode(),
                    capacidadeMax = 1,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { sede = 60, fome = 75, sanidadeInstantanea = 10, bonusRegeneracaoEnergia = 0.2f, tempoBonusRegeneracaoEnergia = 120, boostVelocidade = 0.1f, tempoBoostVelocidade = 60 }, configJSON)
                };
                db.Insert(consumivel);

                    //munição
                item = new Item 
                { 
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Standard 7,62x39mm", 
                    peso = 30, 
                    valor = 3, 
                    imagem = Path.Combine("SVG", "std-762x39mm.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel._762x39.GetHashCode(),
                    capacidadeMax = 30,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { saudeInstantanea = -76, perfuracao = 1.6f}, configJSON)
                };
                db.Insert(consumivel);


                item = new Item 
                { 
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Armor Piercing 7,62x39mm", 
                    peso = 35, 
                    valor = 30, 
                    imagem = Path.Combine("SVG", "std-762x39mm.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel._762x39.GetHashCode(),
                    capacidadeMax = 30,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { saudeInstantanea = -76, perfuracao = 1.95f}, configJSON)
                };
                db.Insert(consumivel);

                item = new Item
                {
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Standard 9x19mm",
                    peso = 11,
                    valor = 1,
                    imagem = Path.Combine("SVG", "std-handgun-ammo.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel._9mm.GetHashCode(),
                    capacidadeMax = 50,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { saudeInstantanea = -90, perfuracao = 1f }, configJSON)
                };
                db.Insert(consumivel);


                item = new Item
                {
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "+P+ 9x19mm",
                    peso = 12,
                    valor = 20,
                    imagem = Path.Combine("SVG", "std-handgun-ammo.svg")
                };
                db.Insert(item);

                consumivel = new Consumivel
                {
                    Item_ID = item.ID,
                    tipoConsumivel = TipoConsumivel._9mm.GetHashCode(),
                    capacidadeMax = 50,
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { saudeInstantanea = -90, perfuracao = 1.25f }, configJSON)
                };
                db.Insert(consumivel);

                //Anexo
                item = new Item
                {
                    tipoItem = TipoItem.Anexo.GetHashCode(),
                    nome = "OKP-7",
                    peso = 300,
                    valor = 300,
                    imagem = Path.Combine("SVG", "red-dot-sight.svg")
                };
                db.Insert(item);

                anexo = new Anexo
                {
                    Item_ID = item.ID,
                    tipoAnexo = TipoAnexo.AK100.GetHashCode(),
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { boostPrecisao = 0.25f, boostErgonomia = 0.05f, boostVelocidadeMira = 0.15f }, configJSON)
                };
                db.Insert(anexo);


                item = new Item
                {
                    tipoItem = TipoItem.Anexo.GetHashCode(),
                    nome = "PU Mosin Scope",
                    peso = 300,
                    valor = 1000,
                    imagem = Path.Combine("SVG", "sniper-sight.svg")
                };
                db.Insert(item);

                anexo = new Anexo
                {
                    Item_ID = item.ID,
                    tipoAnexo = TipoAnexo.AK100.GetHashCode(),
                    efeito = JsonConvert.SerializeObject(new EfeitoAtributos { boostPrecisao = 0.5f, boostErgonomia = -0.15f, boostVelocidadeMira = -0.10f }, configJSON)
                };
                db.Insert(anexo);

                //Carregador
                item = new Item
                {
                    tipoItem = TipoItem.Carregador.GetHashCode(),
                    nome = "Standard AK mag",
                    peso = 330,
                    valor = 100,
                    imagem = Path.Combine("SVG", "AK-mag.svg")
                };
                db.Insert(item);

                carregador = new Carregador
                {
                    Item_ID = item.ID,
                    capacidadeMax = 30,
                    tipoMunicao = TipoConsumivel._762x39.GetHashCode(),
                    tipoAnexo = TipoAnexo.AK100.GetHashCode() + TipoAnexo.AK200.GetHashCode()
                };
                db.Insert(carregador);


                item = new Item
                {
                    tipoItem = TipoItem.Carregador.GetHashCode(),
                    nome = "Small standard AK mag",
                    peso = 240,
                    valor = 60,
                    imagem = Path.Combine("SVG", "AK-small-mag.svg")
                };
                db.Insert(item);

                carregador = new Carregador
                {
                    Item_ID = item.ID,
                    capacidadeMax = 20,
                    tipoMunicao = TipoConsumivel._762x39.GetHashCode(),
                    tipoAnexo = TipoAnexo.AK100.GetHashCode() + TipoAnexo.AK200.GetHashCode()
                };
                db.Insert(carregador);


                item = new Item
                {
                    tipoItem = TipoItem.Carregador.GetHashCode(),
                    nome = "std Glock mag",
                    peso = 50,
                    valor = 30,
                    imagem = Path.Combine("SVG", "GlockMagazine.svg")
                };
                db.Insert(item);

                carregador = new Carregador
                {
                    Item_ID = item.ID,
                    capacidadeMax = 15,
                    tipoMunicao = TipoConsumivel._9mm.GetHashCode(),
                    tipoAnexo = TipoAnexo.Handgun.GetHashCode()
                };
                db.Insert(carregador);


                //Armadura
                item = new Item
                {
                    tipoItem = TipoItem.Armadura.GetHashCode(),
                    nome = "Level IIA vest",
                    peso = 1450,
                    valor = 1200,
                    imagem = Path.Combine("SVG", "kevlar-vest.svg")
                };
                db.Insert(item);

                armadura = new Armadura
                {
                    Item_ID = item.ID,
                    tipoProtecao = TipoArmadura.Colete.GetHashCode(),
                    cobertura = 0.85f,
                    durabilidadeMax = 300,
                    protecao = 1f,
                    absorção = 0.75f
                };
                db.Insert(armadura);

                
                item = new Item
                {
                    tipoItem = TipoItem.Armadura.GetHashCode(),
                    nome = "Level II vest",
                    peso = 1750,
                    valor = 1600,
                    imagem = Path.Combine("SVG", "kevlar-vest.svg")
                };
                db.Insert(item);

                armadura = new Armadura
                {
                    Item_ID = item.ID,
                    tipoProtecao = TipoArmadura.Colete.GetHashCode(),
                    cobertura = 0.85f,
                    durabilidadeMax = 300,
                    protecao = 1.25f,
                    absorção = 0.68f
                };
                db.Insert(armadura);


                item = new Item
                {
                    tipoItem = TipoItem.Armadura.GetHashCode(),
                    nome = "Level IIIA vest",
                    peso = 2000,
                    valor = 2100,
                    imagem = Path.Combine("SVG", "kevlar-vest.svg")
                };
                db.Insert(item);

                armadura = new Armadura
                {
                    Item_ID = item.ID,
                    tipoProtecao = TipoArmadura.Colete.GetHashCode(),
                    cobertura = 0.85f,
                    durabilidadeMax = 250,
                    protecao = 1.5f,
                    absorção = 0.63f
                };
                db.Insert(armadura);

                //Mochila
                item = new Item
                {
                    tipoItem = TipoItem.Mochila.GetHashCode(),
                    nome = "Mochila escolar",
                    peso = 500,
                    valor = 100,
                    imagem = Path.Combine("SVG", "backpack.svg")
                };
                db.Insert(item);

                mochila = new Mochila
                {
                    Item_ID = item.ID,
                    capacidadeBase = 15,
                    espacoBase = 5,
                    pesoMaximo = 15000
                };
                db.Insert(mochila);


                item = new Item
                {
                    tipoItem = TipoItem.Mochila.GetHashCode(),
                    nome = "Traveler's backpack",
                    peso = 800,
                    valor = 400,
                    imagem = Path.Combine("SVG", "traveler-backpack.svg")
                };
                db.Insert(item);

                mochila = new Mochila
                {
                    Item_ID = item.ID,
                    capacidadeBase = 22,
                    espacoBase = 8,
                    pesoMaximo = 30000
                };
                db.Insert(mochila);


                item = new Item
                {
                    tipoItem = TipoItem.Mochila.GetHashCode(),
                    nome = "Militar backpack",
                    peso = 1000,
                    valor = 800,
                    imagem = Path.Combine("SVG", "big-backpack.svg")
                };
                db.Insert(item);

                mochila = new Mochila
                {
                    Item_ID = item.ID,
                    capacidadeBase = 30,
                    espacoBase = 12,
                    pesoMaximo = 35000
                };
                db.Insert(mochila);

                //Explosivo
                item = new Item 
                { 
                    tipoItem = TipoItem.Explosivo.GetHashCode(),
                    nome = "M1 Granade", 
                    peso = 300, 
                    valor = 200, 
                    imagem = Path.Combine("SVG", "granade.svg")
                };
                db.Insert(item);

                explosivo = new Explosivo
                {
                    Item_ID = item.ID,
                    tipoExplosivo = TipoExplosivo.arremessavel.GetHashCode(),
                    dano = 100,
                    raio = 1000,
                    ignicao = 5,
                };
                db.Insert(explosivo);


                Debug.Log("[Servidor] Itens de exemplo inseridos com sucesso no catálogo!");
            }
            else
            {
                Debug.Log($"[Servidor] Tabela 'Item' já contém {quantidade} registros. Pulo de preenchimento.");
            }
        }

        // Log da tabela e visualizar como o banco de dados está
        private void LogTabelaItens()
        {
            var db = DatabaseService.Instance.Connection;
            
            // Resgata todos os itens cadastrados no banco de dados
            var itens = db.Table<Item>().ToList();
            Debug.Log("=== LOG DA TABELA DE ITENS ===");  
            foreach (var item in itens)
            {
                Debug.Log($"ID: {item.ID} | Nome: {item.nome} | Tipo: { Enum.GetName(typeof(TipoItem), item.tipoItem.GetHashCode()) } | Peso: {item.peso}g | Valor: ${item.valor} | Caminho: {item.imagem}");
            }
            Debug.Log("==============================================");


            var armas = db.Table<Arma>().ToList();
            Debug.Log("=== LOG DA TABELA DE ARMAS ===");
            foreach (var arma in armas)
            {
                Debug.Log($"ID: {arma.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == arma.Item_ID).nome } | Tipo Anexo: { Enum.GetName(typeof(TipoAnexo), arma.tipoAnexo.GetHashCode()) } | Tipo Munição: { Enum.GetName(typeof(TipoConsumivel), arma.tipoMunicao.GetHashCode()) } | Compatibilidade: { Convert.ToString(arma.compatibilidade, 2) } | Cadência: { arma.cadencia }DPM | Velocidade: { arma.velocidade }m/s | Ergonomia: { arma.ergonomia } | Precisão (dispersão em graus): { arma.precisao }");
            }
            Debug.Log("==============================================");

            var consumiveis = db.Table<Consumivel>().ToList();
            Debug.Log("=== LOG DA TABELA DE CONSUMÍVEIS ===");
            foreach (var cons in consumiveis)
            {
                Debug.Log($"ID: {cons.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == cons.Item_ID).nome } | Tipo Consumível: { Enum.GetName(typeof(TipoConsumivel), cons.tipoConsumivel.GetHashCode()) } | Capacidade máxima: { cons.capacidadeMax } | Efeitos: { cons.efeito }");
            }
            Debug.Log("==============================================");

            var anexos = db.Table<Anexo>().ToList();
            Debug.Log("=== LOG DA TABELA DE ANEXOS ===");
            foreach (var anexo in anexos)
            {
                Debug.Log($"ID: {anexo.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == anexo.Item_ID).nome } | Tipo Anexo: { Enum.GetName(typeof(TipoAnexo), anexo.tipoAnexo.GetHashCode()) } | Efeitos: { anexo.efeito }");
            }
            Debug.Log("==============================================");

            var carregadores = db.Table<Carregador>().ToList();
            Debug.Log("=== LOG DA TABELA DE CARREGADORES ===");
            foreach (var carregador in carregadores)
            {
                Debug.Log($"ID: {carregador.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == carregador.Item_ID).nome } | Tipo Munição: { Enum.GetName(typeof(TipoConsumivel), carregador.tipoMunicao.GetHashCode()) } | Capacidade máxima: { carregador.capacidadeMax }");
            }
            Debug.Log("==============================================");

            var armaduras = db.Table<Armadura>().ToList();
            Debug.Log("=== LOG DA TABELA DE ARMADURA ===");
            foreach (var armadura in armaduras)
            {
                Debug.Log($"ID: {armadura.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == armadura.Item_ID).nome } | Tipo Anexo: { Enum.GetName(typeof(TipoArmadura), armadura.tipoProtecao.GetHashCode()) } | Nível de proteção: { armadura.protecao } | Absorção: { armadura.absorção } | Cobertura: { armadura.cobertura } | Durabilidade Máxima: { armadura.durabilidadeMax }");
            }
            Debug.Log("==============================================");

            var mochilas = db.Table<Mochila>().ToList();
            Debug.Log("=== LOG DA TABELA DE ANEXOS ===");
            foreach (var mochila in mochilas)
            {
                Debug.Log($"ID: {mochila.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == mochila.Item_ID).nome } | Capacidade máxima: { mochila.capacidadeBase } | Espaço base: { mochila.espacoBase } slots | Capacidade de peso {mochila.pesoMaximo}g");
            }
            Debug.Log("==============================================");

            var explosivos = db.Table<Explosivo>().ToList();
            Debug.Log("=== LOG DA TABELA DE ANEXOS ===");
            foreach (var explosivo in explosivos)
            {
                string temp = (explosivo.angulo == 0) || (explosivo.angulo == null) ? "Explosão radial" : $"{explosivo.angulo}°";
                Debug.Log($"ID: {explosivo.Item_ID} | Nome: { itens.FirstOrDefault(x => x.ID == explosivo.Item_ID).nome } | Tipo Anexo: { Enum.GetName(typeof(TipoExplosivo), explosivo.tipoExplosivo.GetHashCode()) } | Dano: { explosivo.dano } | Raio de alcance: {explosivo.raio}cm | Tempo de ignição: {explosivo.ignicao }s | Ângulo de projeção: {temp}");
            }
            Debug.Log("==============================================");

        }
    }
}