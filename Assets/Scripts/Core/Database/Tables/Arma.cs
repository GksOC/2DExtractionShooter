using SQLite4Unity3d;

[Table("Arma")]
public class Arma
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull] //Enum
    public int tipoAnexo { get; set; }

    [NotNull] //Enum
    public int tipoMunicao { get; set; } //tipoConsumivel

    [NotNull] //Enum
    public int compatibilidade { get; set; } // (bitwise)

    [NotNull]
    public int cadencia { get; set; }

    [NotNull]
    public int velocidade { get; set; }

    [NotNull]
    public int ergonomia { get; set; }
    
    [NotNull]
    public float precisao { get; set; }
}

