using CAGE.Application.Dtos;
using CAGE.Application.UseCases.CreateGame;
using CAGE.Domain.Entities;
using CAGE.Domain.Services;
using CleanArchitecture.Services;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CAGE.Tests.Unit.Application.UseCases.CreateGame
{

    public class CreateGameInteractorTests
    {

        #region - - - - - - Fields - - - - - -

        private readonly Mock<IGameLogic> m_MockGameLogic = new();
        private readonly Mock<ICreateGameOutputPort> m_MockOutputPort = new();

        private readonly CreateGameInputPort m_InputPort = new() { Players = new[] { new NewPlayerDto("player1") } };
        private readonly IUseCaseInteractor<CreateGameInputPort, ICreateGameOutputPort> m_Interactor;
        private GameDto? m_PresentedDto;

        #endregion Fields

        #region - - - - - - Constructors - - - - - -

        public CreateGameInteractorTests()
        {
            this.m_Interactor = new CreateGameInteractor(this.m_MockGameLogic.Object);

            _ = this.m_MockGameLogic.SetupProperty(mock => mock.CurrentGame);

            _ = this.m_MockOutputPort
                    .Setup(mock => mock.PresentNewGameAsync(It.IsAny<GameDto>(), default))
                    .Callback((GameDto dto, CancellationToken c) => this.m_PresentedDto = dto);
        }

        #endregion Constructors

        #region - - - - - - HandleAsync Tests - - - - - -

        [Fact]
        public async Task HandleAsync_CurrentGameDoesNotExist_SetsAndPresentsNewGame()
        {
            // Arrange
            var _ExpectedGame = new Game();
            _ExpectedGame.Players.Add(new Player { Name = "player1" });

            var _ExpectedDto = new GameDto(_ExpectedGame);

            // Act
            await this.m_Interactor.HandleAsync(this.m_InputPort, this.m_MockOutputPort.Object, default);

            // Assert
            _ = this.m_MockGameLogic.Object.CurrentGame.Should().BeEquivalentTo(_ExpectedGame);
            _ = this.m_PresentedDto.Should().BeEquivalentTo(_ExpectedDto);
        }

        [Fact]
        public async Task HandleAsync_CurrentGameExists_OverridesCurrentGameAndPresentsNewGame()
        {
            // Arrange
            this.m_MockGameLogic.Object.CurrentGame = new Game();

            var _ExpectedGame = new Game();
            _ExpectedGame.Players.Add(new Player { Name = "player1" });

            var _ExpectedDto = new GameDto(_ExpectedGame);

            // Act
            await this.m_Interactor.HandleAsync(this.m_InputPort, this.m_MockOutputPort.Object, default);

            // Assert
            _ = this.m_MockGameLogic.Object.CurrentGame.Should().BeEquivalentTo(_ExpectedGame);
            _ = this.m_PresentedDto.Should().BeEquivalentTo(_ExpectedDto);
        }

        #endregion HandleAsync Tests

    }

}
