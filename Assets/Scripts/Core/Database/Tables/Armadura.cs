using SQLite4Unity3d;

[Table("Armadura")]
public class Armadura
{
    [PrimaryKey]
    public int Item_ID { get; set; }

    [NotNull] //Enum
    public int tipoProtecao { get; set; } 

    [NotNull]
    public float protecao { get; set; } //o quão resistente é contra penetração
    /* referência: 1f = IIA, 1.25f = II, 1.5f = IIIA, 2f = III, 2.5f = IV
     * 1f    = 9x19mm HP ou .40S&W
     * 1.25f = 9x19mm +P+ ou .357 Magnum
     * 1.5f  = .357 SIG ou .44 Magnum
     * 2f    = 7,62x51mm
     * 2.5f  = 7,62x63mm AP ou .300WM
     * >3f = .300WM AP
     */

    [NotNull]
    public float absorção { get; set; } 
    /*  quanto mais próximo de 0 mais reduz o dano.
        se for próximo de 1 não há redução de dano.
        valores superiores a 1 multiplica o dano (efeito oposto).
    */

    [NotNull]
    public float cobertura { get; set; }

    [NotNull]
    public int durabilidadeMax { get; set; } //define o quanto consegue absorver de dano
    /* O valor da durabilidade é um float que fica entre 0f a 1f, 0% e 100% respectivamente
     * o dano infligido é calculado em cima do que seria o valor da durabilidade maxima, ou seja:
     * 
     * danoTotal = dano*( (perfuracao² - protecao²)*absorcao² );
     * durabilidade -= (dano + danoTotal <= 0) ? 0.01f : (dano + danoTotal)/durabilidadeMax;
     *
     */ 
}
