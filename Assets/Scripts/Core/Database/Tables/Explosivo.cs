using SQLite4Unity3d;

[Table("Explosivo")]
public class Explosivo
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull]
    public int tipoExplosivo { get; set; }

    [NotNull]
    public int dano { get; set; }

    [NotNull]
    public int raio { get; set; }

    [NotNull]
    public int ignicao { get; set; }

    [NotNull]
    public int angulo { get; set; }
}