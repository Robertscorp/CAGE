using CleanArchitecture.Services;

namespace CAGE.Application.UseCases.CreateGame
{

    public class CreateGameInputPort : IUseCaseInputPort<ICreateGameOutputPort>
    {

        #region - - - - - - Properties - - - - - -

        public NewPlayerDto[] Players { get; set; } = Array.Empty<NewPlayerDto>();

        #endregion Properties

    }

    public class NewPlayerDto
    {

        #region - - - - - - Constructors - - - - - -

        public NewPlayerDto(string name)
            => this.Name = name;

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public string Name { get; }

        #endregion Properties

    }

}
