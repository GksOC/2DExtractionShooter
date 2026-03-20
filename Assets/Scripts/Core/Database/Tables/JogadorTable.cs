using SQLite4Unity3d;

[Table("Jogador")]
public class JogadorTable
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [NotNull, Unique]
    public int corpo_ID { get; set; }

    [NotNull]
    public int nome { get; set; }
}
