using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManagement.Params;

namespace DatabaseManagement
{
    public class BubblesGameManager
    {
        private Game game;
        private Player player;
        private Dictionary<String,GameParam> gameParams = new Dictionary<string,GameParam>();
        private Dictionary<String,GameResult> gameResults = new Dictionary<string,GameResult>();

        public BubblesGameManager()
        {

        }

        public BubblesGameManager(String playerName)
        {
            this.GetGame();
            this.GetPlayer(playerName);
        }

        public BubblesGameManager(Player player)
        {
            this.GetGame();
            this.player = player;
        }

        public void SaveGameResult(BubblesGameParams bgp)
        {
            using (var context = new GameModelContext())
            {
                DateTime date = DateTime.Now;

                var history = new History
                {
                    id_game = this.game.id,
                    id_player = this.player.id,
                    date = date
                };

                history.HistoryParams.Add(new HistoryParam{
                    id_game_params = this.gameParams["level"].id,
                    value = bgp.Level
                });


                List<HistoryResult> historyResults = new List<HistoryResult>();
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["success"].id,
                    value = bgp.Success
                });
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["time"].id,
                    value = bgp.Time
                });
                foreach (HistoryResult item in historyResults)
                {
                    history.HistoryResults.Add(item);
                }

                context.Histories.Add(history);
                context.SaveChanges();
            }
        }

        private void GetGame()
        {
            using (var context = new GameModelContext())
            {
                var game = context.Games
                    .Where(b => b.name == "BubblesGame").FirstOrDefault();
                this.game = game as Game;

                foreach (GameParam item in game.GameParams)
                {
                    this.gameParams.Add(item.name, item);
                }
                foreach (GameResult item in game.GameResults)
                {
                    this.gameResults.Add(item.name, item);
                }
            }
        }

        private void GetPlayer(String playerName)
        {
            using (var context = new GameModelContext())
            {
                var user = context.Players
                    .Where(b => b.name == playerName).FirstOrDefault();
                this.player = user as Player;
            }
        }
    }
}
