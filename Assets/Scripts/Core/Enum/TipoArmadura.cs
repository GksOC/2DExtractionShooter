
enum TipoArmadura 
{
    Capacete = 0,
    Colete = 1, //proteção básica IIA ou IIIA
    ChestRig = 2,
    ChestPlate = 3, //armadura de placa simples
    IBA = 4, //armadura nível III de maior cobertura do tronco e pescoço
    DAPS = 5, //proteção adicional para os braços
    IOTV = 6 //armadura integral 

    /*
     * Regras de compatibilidade:
     * ChestPlate, IBA e IOTV são exclusivas (não pode haver uso combinado).
     * IOTV não é compatível com DAPS pois aqui é assumido que estão já integrados.
     * Apenas ChestPlate e Colete são compatíveis entre si. É assumido que IBA e IOTV tenham essa proteção extra integrada.
     */ 
}