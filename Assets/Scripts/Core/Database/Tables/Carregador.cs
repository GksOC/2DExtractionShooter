using SQLite4Unity3d;

[Table("Carregador")]
public class Carregador
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull] //Enum TipoConsumivel
    public int tipoMunicao { get; set; }
    
    [NotNull] //Enum
    public int tipoAnexo { get; set; }

    [NotNull]
    public int capacidadeMax { get; set; }
}