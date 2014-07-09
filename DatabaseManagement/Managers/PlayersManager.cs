using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement.Managers
{
    public class PlayersManager
    {
        private List<Player> playerList;

        public List<Player> PlayerList
        {
            get { return playerList; }
            set { playerList = value; }
        }

        public PlayersManager()
        {
            this.GetPlayers();
        }

        private void GetPlayers()
        {
            using (var context = new GameModelContext())
            {
                var players = context.Players;

                playerList = new List<Player>();
                foreach (Player item in players)
                {
                    playerList.Add(item);
                }
            }
        }
    }
}
