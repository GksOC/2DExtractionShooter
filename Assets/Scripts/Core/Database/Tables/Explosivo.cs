using SQLite4Unity3d;

[Table("Explosivo")]
public class Explosivo
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull] //Enum
    public int tipoExplosivo { get; set; }

    [NotNull]
    public int dano { get; set; }

    [NotNull]
    public int raio { get; set; } //em centímetros

    [NotNull]
    public int ignicao { get; set; }

    public float? angulo { get; set; }
}