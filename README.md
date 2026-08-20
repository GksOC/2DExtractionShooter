# Protótipo para implantar um Banco de Dados SQL em jogo Unity3D
Este é um jogo Unity3D com um sistema de autoridade de dados em SQLite. 
O objetivo é criar um jogo com alto nível de integridade referencial para impedir manipulações de trapaceiros. 
Também é um projeto de Banco de Dados para faculdade.

## Conclusões prévias:
### Dificuldades:
A implantação de um sistema de autoridade de dados se demonstrou um desafio técnico que exige mais planejamento especializado.
Apesar de todos os detalhes do Modelo Lógico ajudar na implantação, questões técnicas específicas da engine se tornam um grande desafio de ser contornado sem o conhecimento prévio.

### Alteraçoes?
A ideia deste projeto agora é deixar de lado o SQL e  migrar para o NoSQL (LiteDB/MongoDB) que é nativo, mais rápido, flexível e escalável.
