using SQLite4Unity3d;

[Table("Mochila")]
public class Mochila
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull]
    public int capacidadeBase { get; set; }

    [NotNull]
    public int espacoBase { get; set; }
}
