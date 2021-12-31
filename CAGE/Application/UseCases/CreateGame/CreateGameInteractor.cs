using CAGE.Application.Dtos;
using CAGE.Domain.Entities;
using CAGE.Domain.Services;
using CleanArchitecture.Services;

namespace CAGE.Application.UseCases.CreateGame
{

    public class CreateGameInteractor : IUseCaseInteractor<CreateGameInputPort, ICreateGameOutputPort>
    {

        #region - - - - - - Fields - - - - - -

        private readonly IGameLogic m_GameLogic;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public CreateGameInteractor(IGameLogic gameLogic)
            => this.m_GameLogic = gameLogic;

        #endregion Constructors

        #region - - - - - - Methods - - - - - -

        Task IUseCaseInteractor<CreateGameInputPort, ICreateGameOutputPort>.HandleAsync(
            CreateGameInputPort inputPort,
            ICreateGameOutputPort outputPort,
            CancellationToken cancellationToken)
        {
            this.m_GameLogic.CurrentGame = new Game();

            foreach (var _Player in inputPort.Players)
                this.m_GameLogic.CurrentGame.Players.Add(new Player() { Name = _Player.Name });

            return outputPort.PresentNewGameAsync(new GameDto(this.m_GameLogic.CurrentGame), cancellationToken);
        }

        #endregion Methods

    }

}
