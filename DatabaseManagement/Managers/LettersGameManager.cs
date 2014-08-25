using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class LettersGameManager
    {
        private Game game;
        private Player player;
        private Dictionary<String, GameParam> gameParams = new Dictionary<string, GameParam>();
        private Dictionary<String, GameResult> gameResults = new Dictionary<string, GameResult>();


        public LettersGameManager()
        {

        }

        public LettersGameManager(Player player)
        {
            this.player = player;
            this.GetGame();
        }

        public void SaveGameResult(LettersGameParams lgp)
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
                    id_game_params = this.gameParams["lettersCount"].id,
                    value = lgp.LettersCount
                });
                historyParams.Add(new HistoryParam
                {
                    id_game_params = this.gameParams["level"].id,
                    value = lgp.Level
                });

                foreach (HistoryParam item in historyParams)
                {
                    history.HistoryParams.Add(item);
                }

                List<HistoryResult> historyResults = new List<HistoryResult>();
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["correctTrials"].id,
                    value = lgp.CorrectTrials
                });
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["failures"].id,
                    value = lgp.Failures
                });
                historyResults.Add(new HistoryResult
                {
                    id_game_results = this.gameResults["time"].id,
                    value = lgp.Time
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
                    .Where(b => b.name == "LettersGame").FirstOrDefault();
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
    }
}
