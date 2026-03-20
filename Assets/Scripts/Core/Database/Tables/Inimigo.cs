using SQLite4Unity3d;

[Table("Inimigo")]
public class Inimigo
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [NotNull, Unique]
    public int Corpo_ID { get; set; }

    [NotNull]
    public string nome { get; set; }

    [NotNull]
    public int alcanceVisao { get; set; }

    [NotNull]
    public int anguloVisao { get; set; }

    [NotNull] 
    public int tempoReacao { get; set; }

    [NotNull]
    public float precisao { get; set; }
}
