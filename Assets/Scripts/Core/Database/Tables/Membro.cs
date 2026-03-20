using SQLite4Unity3d;

[Table("Membro")]
public class Membro 
{
    [PrimaryKey, AutoIncrement] 
    public int ID { get; set; }

    [NotNull]
    public string nome { get; set; }

    [NotNull]
    public bool sangrando { get; set; }

    [NotNull]
    public bool infeccionado { get; set; }

    [NotNull]
    public bool quebrado { get; set; }

    [NotNull]
    public int saude { get; set; }
}
