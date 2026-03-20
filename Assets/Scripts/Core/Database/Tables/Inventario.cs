using SQLite4Unity3d;

[Table("Inventario")]
public class Inventario{
    [NotNull, PrimaryKey]
    public int origem_ID { get; set; }

    [NotNull]
    public int capacidade { get; set; }

    [NotNull]
    public int espaco { get; set; }
}
