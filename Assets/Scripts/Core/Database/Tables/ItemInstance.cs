using SQLite4Unity3d;

[Table("ItemInstance")]
public class ItemInstance
{ 
    [PrimaryKey, AutoIncrement, NotNull] 
    public int ID { get; set; }

    [NotNull]
    public int Item_ID { get; set; }

    [NotNull]
    public int espaco { get; set; } 

    [NotNull]
    public float durabilidade { get; set; }

    [NotNull]
    public float qualidade { get; set; }
}
