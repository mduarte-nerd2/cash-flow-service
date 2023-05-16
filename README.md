# cash-flow-service
Aplicação para controle de fluxo de caixa desenvolvida com C# 7.0 e arquitetura Hexagonal para os serviços.


A arquitetura hexagonal, também conhecida como Arquitetura de Ports and Adapters, traz as seguintes vantagens:

- Separação clara de preocupações entre o domínio da aplicação e os detalhes técnicos.
- Colocação do domínio da aplicação no centro, facilitando sua compreensão e manutenção.
- Utilização do conceito de Ports and Adapters para definir interfaces e adaptadores, permitindo substituição fácil de dependências externas.
- Maior testabilidade, com possibilidade de testar as regras de negócio de forma isolada e substituir implementações externas por versões simuladas ou mockadas.
- Estrutura modular e organizada que facilita a manutenção e evolução do sistema.
- Redução do acoplamento entre componentes, tornando-os independentes e substituíveis.
- Flexibilidade e escalabilidade para adicionar recursos e integrar com serviços externos.
Em resumo, a arquitetura hexagonal oferece maior testabilidade, manutenibilidade, desacoplamento, flexibilidade e escalabilidade, resultando em sistemas mais robustos, modularizados e fáceis de evoluir.

![design-hexagonal-architecture](https://github.com/mduarte-nerd2/cash-flow-service/assets/133377343/55f726d3-5c1a-4a6c-8224-42cc85229b9b)

É uma Aplicação desenvolvida para atender ao micro-dominio de controle de Livro Caixa e suas Transações. Todo dia um novo Livro Caixa é aberto, e ele serve para registrar todas as transações que ocorram,
sejam de débito ou crédito, A Qualquer momento o usuário da aplicação pode criar transações que são associadas ao Livro Caixa do dia. Ao final o usuário pode solicitar um extrato total de todas as transações.
Abaixo o modelo de domínio do microsserviço.
![cash-flow-service-domain](https://github.com/mduarte-nerd2/cash-flow-service/assets/133377343/3d09da42-6070-4c54-a71c-f381b8bcdc90)


#Principais Casos de Uso da Aplicação:
- Criar Cash Book;
- Criar Transação de Débito ou Crédito;
- Listar Extrato de Transação;

# Sequencia de Chamadas
Abaixo as principais requisições feitas. Todas as Requisições seguem o Padrão de recursos Rest.
![cash-flow-service-sequence](https://github.com/mduarte-nerd2/cash-flow-service/assets/133377343/8913d016-9173-45e6-b34b-985bbcb9bfb4)

