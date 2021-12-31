using CAGE.Domain.Entities;

namespace CAGE.Application.Dtos
{

    public class PlayerDto
    {

        #region - - - - - - Constructors - - - - - -

        public PlayerDto(Player player)
        {
            this.Name = player.Name;
            this.PlayerID = player.ID;
        }

        #endregion Constructors

        #region - - - - - - Properties - - - - - -

        public string Name { get; set; }

        public EntityID PlayerID { get; set; }

        #endregion Properties

    }

}
