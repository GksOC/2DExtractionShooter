using SQLite4Unity3d;
public class Item 
{
    [PrimaryKey, AutoIncrement] 
    public int ID { get; set; }

    [NotNull] //Enum
    public int tipoItem { get; set; }

    [NotNull]
    public string nome { get; set; }

    [NotNull]
    public int peso { get; set; }

    [NotNull]
    public int valor { get; set; }

    [NotNull]
    public string imagem { get; set; } //diretório para um .svg
}
