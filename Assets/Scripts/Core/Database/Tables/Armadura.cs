using SQLite4Unity3d;

[Table("Armadura")]
public class Armadura
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull]
    public int tipoProtecao { get; set; }

    [NotNull]
    public float protecao { get; set; }

    [NotNull]
    public float cobertura { get; set; }

    [NotNull]
    public int durabilidadeMax {get; set; }
}
