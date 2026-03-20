using SQLite4Unity3d;

[Table("Corpo")]
public class Corpo
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [NotNull]
    public int nivel { get; set; }

    [NotNull]
    public int xp { get; set; }

    [NotNull]
    public int energia { get; set; }

    [NotNull]
    public int energiaMax { get; set; }

    [NotNull]
    public int sanidade { get; set; }

    [NotNull]
    public int sanidadeMax { get; set; }

    [NotNull]
    public int fome { get; set; }

    [NotNull]
    public int sede { get; set; }

    [NotNull]
    public int sono { get; set; }
}
