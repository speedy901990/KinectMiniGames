using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement.Managers
{
    public class PlayersManager
    {
        public List<Player> PlayerList { get; set; }

        public PlayersManager()
        {
            GetPlayers();
        }

        private void GetPlayers()
        {
            using (var context = new GameModelContainer())
            {
                var players = context.Players;

                PlayerList = new List<Player>(players);
            }
        }
    }
}
