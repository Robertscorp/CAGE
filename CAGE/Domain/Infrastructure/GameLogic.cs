using CAGE.Domain.Entities;
using CAGE.Domain.Services;

namespace CAGE.Domain.Infrastructure
{

    public class GameLogic : IGameLogic
    {

        #region - - - - - - Properties - - - - - -

        Game? IGameLogic.CurrentGame { get; set; }

        #endregion Properties

    }

}
