[System.Serializable]
public class EfeitoAtributos
{
    /*  COMO FUNCIONA OS VALORES:
        - valores "(...)Instantaneo" é um valor que adiciona diretamente no valor. Ex.: energia += energiaInstantanea
        - valores "tempo" refere quanto tempo o efeito dura em segundos! Lembre-se que tempo SEMPRE será contado em INTEIROS.
        - valores "regeneracao" são acrescidos com o tempo por segundo! Ex.: 0.5f quer dizer que a cada 2 segundos vai aumentar uma unidade do int.
        - "boost" implica que é um multiplicador do valor de referência. Ex.: precisao = precisaoBase * (1 + boostPrecisao)
        - "bonus" implica que já há um valor natural que modifica o status. Modifique com cuidado para não impactar na experiência
            Ex.: acumuladorEnergia += regeneracaoEnergiaBase + (1 * bonusRegeneracaoEnergia)
        - tanto "bonus" quanto "boost" são multiplicadores normalizados. 0.1 = +10%, -0.1 = -10%.
        - NÃO É NECESSÁRIO COLOCAR VALOR EM TODOS. Os status são calculados somando todos os buffs e debufs para realizar apenas uma última operação.
        - NÃO CONFUNDIR "bonus" e "boost" com "regeneracao".
        - "bonus(...)Maxim@" se refere à capacidade de modificar o limite de algum status.
        - Apenas itens consumíveis precisam de ter valor no "tempo" diferente de zero. Efeitos dos Anexos são aplicados quando há alguma mudança.
     */

    //efeito no armamento
    public float boostPrecisao;
    public int tempoBoostPrecisao;
    public float boostVelocidadeRecarga;
    public int tempoBoostVelocidadeRecarga;
    public float boostErgonomia; //verificar se o multiplicador faz alguma diferença significativa pois "ergonomia" é INT min 1 e max 100
    public int tempoBoostErgonomia;
    public float boostVelocidadeMira;
    public int tempoBoostVelocidadeMira;

    //efeito no corpo
    public float boostVelocidade;
    public int tempoBoostVelocidade;
    public int energiaInstanea;
    public float bonusRegeneracaoEnergia;
    public int tempoBonusRegeneracaoEnergia;
    public int bonusEnergiaMaxima;
    public int tempoBonusEnergiaMaxima;
    public int sanidadeInstantanea;
    public float regeneracaoSanidade;
    public int tempoRegeneracaoSanidade;
    //a lógica é oposta, se adicionar retarda, se reduzir aumenta. Ex.: sede = 0 → desidratação severa, sede = 100 → muito hidratado
    public int sede;
    public int fome; 
    public int sono;

    //efeito no membro (do corpo)
    public int membroAlvo; //Enum NomeMembro 
    public int saudeInstantanea; //valores negativos podem ser usados para atribuir dano
    public float regeneracaoSaude;
    public int tempoRegeneracaoSaude;
    public int bonusSaudeMaxima; //mecânica de sobrevida (adrenalina).
    public int tempoBonusSaudeMaxima; //ao acabar o tempo, o valor de "bonusSaudeMaxima" deve ser subtraída do status de "saude" e afetar a "sanidade"

    //efeito na balística (o que causa ao ser atingido)
    // public int dano; //USAR O CAMPO "saudeInstantanea"
    public float perfuracao; 
        /* referência: 1f = IIA, 1.25f = II, 1.5f = IIIA, 2f = III, 2.5f = IV
         * 1f    = 9x19mm HP ou .40S&W
         * 1.25f = 9x19mm +P+ ou .357 Magnum
         * 1.5f  = .357 SIG ou .44 Magnum
         * 2f    = 7,62x51mm
         * 2.5f  = 7,62x63mm AP ou .300WM
         * >3f = .300WM AP
         */

    // Aqui pode adicionar dezenas de outros status escaláveis. 
}