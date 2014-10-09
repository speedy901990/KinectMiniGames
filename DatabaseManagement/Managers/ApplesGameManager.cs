using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatabaseManagement.Params;

namespace DatabaseManagement.Managers
{
    public class ApplesGameManager
    {
        private readonly Player _player;

        public ApplesGameManager()
        {

        }

        public ApplesGameManager(Player player)
        {
            _player = player;
        }

        public void SaveGameResult(ApplesGameParams apg)
        {
            using (var context = new GameModelContainer())
            {
                var date = DateTime.Now;

                var game = context.Games.FirstOrDefault(b => b.Name == "ApplesGame");

                if (game == null)
                    return;

                var historyParams = new List<HistoryParam>
                {
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Apples"),
                        Value = apg.Apples.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Colors"),
                        Value = apg.Colors.ToString(CultureInfo.InvariantCulture)
                    },
                    new HistoryParam
                    {
                        GameParam = game.GameParams.FirstOrDefault(param => param.Name == "Baskets"),
                        Value = apg.Baskets.ToString(CultureInfo.InvariantCulture)
                    }
                };

                var historyResults = new List<HistoryResult>
                {
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Correct Trials"),
                        Value = apg.CorrectTrials
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Failures"),
                        Value = apg.Failures
                    },
                    new HistoryResult
                    {
                        GameResult = game.GameResults.FirstOrDefault(result => result.Name == "Time"),
                        Value = apg.Time
                    }
                };

                var history = new History
                {
                    Game = game,
                    Date = date,
                    HistoryParams = historyParams,
                    HistoryResults = historyResults
                };
                var player = context.Players.FirstOrDefault(player1 => player1.Id == _player.Id);
                if (player != null)
                    player.Histories.Add(history);
                context.SaveChanges();
            }
        }
    }
}