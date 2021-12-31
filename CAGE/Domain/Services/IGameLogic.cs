using CAGE.Domain.Entities;

namespace CAGE.Domain.Services
{

    public interface IGameLogic
    {

        #region - - - - - - Properties - - - - - -

        public Game? CurrentGame { get; set; }

        #endregion Properties

    }

}
