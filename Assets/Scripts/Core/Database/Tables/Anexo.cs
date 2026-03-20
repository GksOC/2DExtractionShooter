using SQLite4Unity3d;

[Table("Anexo")]
public class Anexo
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull]
    public int tipoAnexo { get; set; }

    [NotNull]
    public string efeito { get; set; }
}

