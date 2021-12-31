using CAGE.Domain.Entities;

namespace CAGE.Application.Dtos
{

    public class GameDto
    {

        #region - - - - - - Constructors - - - - - -

        public GameDto(Game game)
        {
            this.GameID = game.ID;
            this.Players = game.Players.Select(p => new PlayerDto(p)).ToArray();
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public EntityID GameID { get; set; }

        public PlayerDto[] Players { get; set; }

        #endregion Properties

    }

}
