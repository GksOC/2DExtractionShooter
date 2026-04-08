using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using SQLite4Unity3d;

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

            // Verifica fisicamente se o arquivo do banco já existia antes de abrir o jogo
            if (!File.Exists(dbPath))
            {
                Debug.Log("[Servidor] Banco de dados não encontrado. Gerando novo arquivo SQLite...");
            }
            else
            {
                Debug.Log("[Servidor] Banco de dados existente encontrado. Verificando integridade das tabelas...");
            }
            GerarTabelasBase(); //verifiquei que ambas situações resolvem com a mesma função.
            VerificarEPreencherItens();
            LogTabelaItens();
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
                db.CreateTable<JogadorTable>(); // [16]
                db.CreateTable<Inimigo>(); // [17]

                Debug.Log("[Servidor] Sucesso! Todas as tabelas base foram criadas/validadas no banco de dados.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Servidor] Erro crítico ao gerar as tabelas: {ex.Message}");
            }
        }

        // 1. Método para verificar e preencher a tabela base (Exemplo com Item)
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
                    compatibilidade = Comaptibilidade.bocal.GetHashCode() + Compatibilidade.mira.GetHashCode(),
                    cadencia = 600,
                    velocidade = 670,
                    ergonomia = 65,
                    precisao = 1.0
                };
                db.Insert(arma);

                var item2 = new Item 
                { 
                    tipoItem = TipoItem.Consumivel.GetHashCode(),
                    nome = "Water Bottle", 
                    peso = 500, 
                    valor = 10, 
                    imagem = Path.Combine("SVG", "water-bottle.svg")
                };


                db.Insert(item2);

                Debug.Log("[Servidor] Itens de exemplo inseridos com sucesso no catálogo!");
            }
            else
            {
                Debug.Log($"[Servidor] Tabela 'Item' já contém {quantidade} registros. Pulo de preenchimento.");
            }
        }

        // 2. Método para fazer o Log da tabela e visualizar como o banco de dados está
        private void LogTabelaItens()
        {
            var db = DatabaseService.Instance.Connection;
            
            // Resgata todos os itens cadastrados no banco de dados
            var todosOsItens = db.Table<Item>().ToList();

            Debug.Log("=== LOG DA TABELA DE ITENS (CATÁLOGO BASE) ===");
            
            foreach (var item in todosOsItens)
            {
                // Interpolação de string formatada para facilitar a leitura no console
                Debug.Log($"ID: {item.ID} | Nome: {item.nome} | Tipo (Enum): {item.tipoItem} | Peso: {item.peso}g | Valor: $ {item.valor} | Caminho: {item.imagem}");
            }
            
            Debug.Log("==============================================");
        }
    }
}