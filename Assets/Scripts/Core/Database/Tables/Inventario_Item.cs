using SQLite4Unity3d;

[Table("Inventario_item")]
public class Inventario_Item
{

    [PrimaryKey]
    public int Item_instance_ID { get; set; }

    [Indexed]
    public int Inventario_ID { get; set; }

    [NotNull]
    public bool equipado { get; set; }


    public float? posX { get; set; } //Pode ser NULL


    public float? posY { get; set; } //Pode ser NULL
}
