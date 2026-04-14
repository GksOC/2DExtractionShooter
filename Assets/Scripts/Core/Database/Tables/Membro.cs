using SQLite4Unity3d;

[Table("Membro")]
public class Membro 
{
    [PrimaryKey, AutoIncrement] 
    public int ID { get; set; }

    [NotNull] //Enum NomeMembro
    public int nome { get; set; }

    [NotNull]
    public bool sangrando { get; set; }

    [NotNull]
    public bool infeccionado { get; set; }

    [NotNull]
    public bool quebrado { get; set; }

    [NotNull]
    public int saude { get; set; }
}
