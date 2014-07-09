using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManagement.Params;

namespace DatabaseManagement
{
    public class ApplesGameManager
    {
        private Game game;
        private Player player;
        private Dictionary<String,GameParam> gameParams = new Dictionary<string,GameParam>();
        private Dictionary<String,GameResult> gameResults = new Dictionary<string,GameResult>();

        public ApplesGameManager()
        {

        }

        public ApplesGameManager(String playerName)
        {
            this.GetGame();
            this.GetPlayer(playerName);
        }

        public ApplesGameManager(Player player)
        {
            this.GetGame();
            this.GetPlayer(player);
        }

        public void SaveGameResult(ApplesGameParams apg)
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

                List<HistoryParam> historyParams = new List<HistoryParam>();
                historyParams.Add(new HistoryParam
                {
                    id_game_params = this.gameParams["apples"].id,
                    value = apg.Apples
                });
                historyParams.Add(new HistoryParam
                {
                    id_game_params = this.gameParams["colors"].id,
                    value = apg.Colors
                });
                historyParams.Add(new HistoryParam
                {
                    id_game_params = this.gameParams["baskets"].id,
                    value = apg.Baskets
                });
                foreach (HistoryParam item in historyParams)
                {
                    history.HistoryParams.Add(item);
                }

                List<HistoryResult> historyResults = new List<HistoryResult>();
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["correctTrials"].id,
                    value = apg.CorrectTrials
                });
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["failures"].id,
                    value = apg.Failures
                });
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["time"].id,
                    value = apg.Time
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
                    .Where(b => b.name == "ApplesGame").FirstOrDefault();
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

        private void GetPlayer(Player player)
        {
            this.player = player;
        }
    }
}