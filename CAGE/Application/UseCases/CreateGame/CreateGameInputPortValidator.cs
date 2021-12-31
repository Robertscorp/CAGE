using CAGE.Application.Services;
using FluentValidation;

namespace CAGE.Application.UseCases.CreateGame
{

    public class CreateGameInputPortValidator : BaseValidator<CreateGameInputPort>
    {

        #region - - - - - - Constructors - - - - - -

        public CreateGameInputPortValidator()
        {
            _ = this.RuleFor(inputPort => inputPort.Players).NotEmpty();
            _ = this.RuleForEach(inputPort => inputPort.Players)
                    .ChildRules(player => player.RuleFor(p => p.Name).NotEmpty());
        }

        #endregion Constructors

    }

}
