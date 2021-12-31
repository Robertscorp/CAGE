using CAGE.Application.Common;
using CleanArchitecture.Services;
using FluentValidation;

namespace CAGE.Application.Services
{

    public abstract class BaseValidator<TUseCaseInputPort> :
        AbstractValidator<TUseCaseInputPort>,
        IUseCaseInputPortValidator<TUseCaseInputPort, InputPortValidationResult>
        where TUseCaseInputPort : IUseCaseInputPort<IValidationOutputPort<InputPortValidationResult>>

    {

        #region - - - - - - Methods - - - - - -

        Task<InputPortValidationResult> IUseCaseInputPortValidator<TUseCaseInputPort, InputPortValidationResult>.ValidateAsync(TUseCaseInputPort inputPort, CancellationToken cancellationToken)
            => Task.FromResult(new InputPortValidationResult(this.Validate(inputPort)));

        #endregion Methods
    }

}
