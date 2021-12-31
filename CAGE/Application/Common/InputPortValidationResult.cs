using CleanArchitecture.Services;
using FluentValidation.Results;

namespace CAGE.Application.Common
{

    public class InputPortValidationResult : ValidationResult, IValidationResult
    {

        #region - - - - - - Constructors - - - - - -

        public InputPortValidationResult(ValidationResult validationResult) : base(validationResult.Errors) { }

        #endregion Constructors

    }

}
