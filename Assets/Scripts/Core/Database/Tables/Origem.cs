using SQLite4Unity3d;

[Table("Origem")]
public class Origem {
    [NotNull, PrimaryKey, AutoIncrement] 
    public int ID { get; set; }

    public int? dono_ID { get; set; } // NULL permitido

    [NotNull] //Enum TipoOrigem
    public int tipoOrigem  { get; set; }

    [NotNull]
    public bool permanente { get; set; }
}
