public class ItemInventarioDTO
{
    // Propriedades que mapeiam as colunas que vamos selecionar no SQL
    public string NomeItem { get; set; }
    public int Peso { get; set; }
    public int Valor { get; set; }
    public float Durabilidade { get; set; }
    public float Qualidade { get; set; }
    public int? Quantidade { get; set; }
}