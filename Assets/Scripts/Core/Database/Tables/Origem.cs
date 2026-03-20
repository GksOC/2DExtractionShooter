using SQLite4Unity3d;

[Table("Origem")]
public class Origem {
    [NotNull, PrimaryKey, AutoIncrement] 
    public int ID { get; set; }

    public int? dono_ID { get; set; } // NULL permitido

    [NotNull]
    public int tipoOrigem  { get; set; } //Enum

    [NotNull]
    public bool fisico { get; set; }

    [NotNull]
    public bool permanente { get; set; }
}
