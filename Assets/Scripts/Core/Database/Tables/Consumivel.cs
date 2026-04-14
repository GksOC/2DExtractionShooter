using SQLite4Unity3d;

[Table("Consumivel")]
public class Consumivel 
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull] //Enum
    public int tipoConsumivel { get; set; }

    [NotNull]
    public int capacidadeMax { get; set; }

    [NotNull]
    public string efeito { get; set; }
}
