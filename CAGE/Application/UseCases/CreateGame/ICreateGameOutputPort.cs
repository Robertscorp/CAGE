using CAGE.Application.Common;
using CAGE.Application.Dtos;
using CleanArchitecture.Services;

namespace CAGE.Application.UseCases.CreateGame
{

    public interface ICreateGameOutputPort : IValidationOutputPort<InputPortValidationResult>
    {

        #region - - - - - - Methods - - - - - -

        Task PresentNewGameAsync(GameDto game, CancellationToken cancellationToken);

        #endregion Methods

    }

}
