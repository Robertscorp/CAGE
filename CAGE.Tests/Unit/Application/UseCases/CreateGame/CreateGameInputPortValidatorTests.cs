using CAGE.Application.UseCases.CreateGame;
using FluentValidation.TestHelper;
using Xunit;

namespace CAGE.Tests.Unit.Application.UseCases.CreateGame
{

    public class CreateGameInputPortValidatorTests
    {

        #region - - - - - - Fields - - - - - -

        private readonly CreateGameInputPortValidator m_Validator = new();

        #endregion Fields

        #region - - - - - - Validate Tests - - - - - -

        [Fact]
        public void Validate_GameHasNoPlayers_HasValidationError()
            => _ = this.m_Validator.TestValidate(new CreateGameInputPort()).ShouldHaveValidationErrorFor(ip => ip.Players);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validate_InvalidPlayerNames_HasValidationError(string playerName)
            => _ = this.m_Validator.TestValidate(new CreateGameInputPort()
            {
                Players = new[] { new NewPlayerDto(playerName) }
            }).ShouldHaveValidationErrorFor($"{nameof(CreateGameInputPort.Players)}[0].{nameof(NewPlayerDto.Name)}");

        [Fact]
        public void Validate_ValidInputPort_HasNoValidationErrors()
            => this.m_Validator.TestValidate(new CreateGameInputPort()
            {
                Players = new[] { new NewPlayerDto("player1") }
            }).ShouldNotHaveAnyValidationErrors();

        #endregion Validate Tests

    }

}
