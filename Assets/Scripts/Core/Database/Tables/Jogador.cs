using SQLite4Unity3d;

[Table("Jogador")]
public class Jogador { 
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [NotNull]
    public int Corpo_ID { get; set; }

    [NotNull]
    public string nome { get; set; }
}
