using SQLite4Unity3d;
using UnityEngine;
using System.IO;

public class DatabaseService : MonoBehaviour
{
    // A instância Singleton global
    public static DatabaseService Instance { get; private set; }
    
    // A propriedade que guarda a conexão única
    public SQLiteConnection Connection { get; private set; }

    private void Awake()
    {
        // Proteção do Singleton: destrói cópias indesejadas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantém o serviço vivo em todo o jogo

        // Estabelece a conexão contínua com o banco
        // Nota: Application.persistentDataPath é o local ideal para bancos de dados que sofrerão gravações.
        string dbPath = Path.Combine(Application.persistentDataPath, "ExtractionShooter.db");
        Connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    private void OnApplicationQuit()
    {
        // Fecha a conexão limpa e seguramente ao encerrar o jogo
        Connection?.Close();
    }
}